﻿using System.Numerics;
using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.GLSettings;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Domain.Account.Utility;
using Mapster;
using Serilog;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class EntryService : BaseService<Entry,EntryCreateCommand,EntryUpdateCommand>,IEntryService
{
    private ApplicationDbContext _dbContext;
    private IEntryRepository _repository;
    public EntryService(IEntryRepository repository,ApplicationDbContext dbContext) : base(repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    public override async Task<ApiResponse<Entry>> Create(EntryCreateCommand command, bool isValidate = true)
    {
        try
        {
            var bussinessValidationResult = await ValidateCreate(command);
            if (!bussinessValidationResult.isValid)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.errors
                };
            }
            var entry = command.Adapt<Entry>();
            entry.FinancialTransactions = command.FinancialTransactions;
            
            if (command.Attachments != null && command.Attachments.Any())
                entry.EntryAttachments = command.Attachments.ToAttachment().Select(e => CreateEntryAttachment(e)).ToList();
            
            entry = await _repository.Add(entry);
            entry.FinancialTransactions = [];
        
           return new ApiResponse<Entry>
           {
               IsSuccess = true,
               StatusCode = HttpStatusCode.OK,
               Result = entry
           };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }
    
    public override async Task<ApiResponse<Entry>> Update(EntryUpdateCommand command, bool isValidate = true)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var bussinessValidationResult = await ValidateUpdate(command);
            if (!bussinessValidationResult.isValid || bussinessValidationResult.entity is null)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.errors
                };
            }

            Entry entry = bussinessValidationResult.entity;

            entry.EntryDate = command.EntryDate;
            entry.BranchId = command.BranchId;
            entry.ExchageRate = command.ExchageRate;
            entry.Notes = command.Notes;
            entry.CurrencyId = command.CurrencyId;
            await UpdateEntryAttachments(command, entry);
            await UpdateFinancialTransactions(entry, command.FinancialTransactions);
            entry.FinancialTransactions = [];
            entry.EntryAttachments = null;
            _dbContext.Set<Entry>().Update(entry);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            entry.FinancialTransactions = [];
            return new ApiResponse<Entry>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entry
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    private  async Task UpdateEntryAttachments(EntryUpdateCommand command, Entry entry)
    {
        IEnumerable<EntryAttachment> oldAttachments = await _dbContext.Set<EntryAttachment>()
                                                                .Include(e => e.Attachment)
                                                                .Where(e => e.EntryId == entry.Id).ToListAsync();

        IEnumerable<EntryAttachment> deletedEntryAttachments = oldAttachments.Where(e => !command.Attachments.Any(file => file.AttachmentId == e.Attachment.Id));
        IEnumerable<Attachment> deletedAttachments = oldAttachments.Select(e => e.Attachment);
        IEnumerable<Attachment> newAttachments =  command.Attachments.Where(file => !oldAttachments.Any(e => file.AttachmentId == e.Attachment.Id))
                                                                     .ToAttachment();
        List<Attachment> updatedAttachments = oldAttachments.Select(e=>e.Attachment).Except(deletedAttachments)
                                                            .ToList();

        
        await SyncUpdatedEntryAttachments(updatedAttachments, command.Attachments);
        if (deletedAttachments.Any())
        {
            _dbContext.Set<EntryAttachment>().RemoveRange(deletedEntryAttachments);
            _dbContext.Set<Attachment>().RemoveRange(deletedAttachments);
        }

        var newEntryAttachments = newAttachments.Select(e =>
        {
            var entryAttachment = CreateEntryAttachment(e);
            entryAttachment.EntryId = entry.Id;
            return entryAttachment;
        });
        
        if (newAttachments.Any())
            await _dbContext.Set<EntryAttachment>().AddRangeAsync(newEntryAttachments);

        if (updatedAttachments.Any())
            _dbContext.Set<Attachment>().UpdateRange(updatedAttachments);
        await _dbContext.SaveChangesAsync();
    }

    private EntryAttachment CreateEntryAttachment(Attachment attachment)
        => new EntryAttachment
        {
            Attachment = attachment
        };

    private async Task SyncUpdatedEntryAttachments(List<Attachment> oldAttachments,
        List<AttachmentDto> newAttachments)
    {
        foreach (var oldAttachment in oldAttachments)
        {
            AttachmentDto newAttachment = newAttachments.First(e => e.AttachmentId == oldAttachment.Id);
            newAttachment.CopyTo(oldAttachment);
        }
    }
    
    private async Task UpdateFinancialTransactions(Entry entry, IEnumerable<FinancialTransaction> newFinancialTransactions)
    {
        try
        {
            
            List<FinancialTransaction> existedFinancialTransactions =
                _dbContext.Set<FinancialTransaction>().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<FinancialTransaction> deletedFinancialTransactions =
                existedFinancialTransactions.Where(f => !newFinancialTransactions.Any(e => e.Id == f.Id));

            List<FinancialTransaction> addedFinancialTransactions =
                newFinancialTransactions.Where(f => !existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            addedFinancialTransactions.ForEach(e=>e.EntryId = entry.Id);
            List<FinancialTransaction> updatedFinancialTransactions = newFinancialTransactions
                .Where(f => existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedTransactions(existedFinancialTransactions, updatedFinancialTransactions);
            
            if (addedFinancialTransactions.Any())
                await _dbContext.Set<FinancialTransaction>().AddRangeAsync(addedFinancialTransactions);
            if (updatedFinancialTransactions.Any())
                _dbContext.Set<FinancialTransaction>().UpdateRange(updatedFinancialTransactions);
            if (deletedFinancialTransactions.Any())
                _dbContext.Set<FinancialTransaction>().RemoveRange(deletedFinancialTransactions);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task SyncCreationInfoToUpdatedTransactions(IEnumerable<FinancialTransaction> oldFinancialTransactions,
        List<FinancialTransaction> newFinancialTransactions)
    {
        foreach(var transaction in newFinancialTransactions)
        {
            FinancialTransaction oldTransaction = oldFinancialTransactions.FirstOrDefault(e => e.Id == transaction.Id);
            transaction.CreatedAt = oldTransaction.CreatedAt;
            transaction.CreatedBy = oldTransaction.CreatedBy;
            transaction.EntryId = oldTransaction.EntryId;
        }
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        var result = new EntryNumberDto();

        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= dateTime && e.EndDate > dateTime);
        if (financialPeriod == null)
        {
            return new ApiResponse<EntryNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.Found,
                ErrorMessages = new List<string> {"NotFoundCurrentFinancialPeriod"}
            };
        }

        result.FinancialPeriodId = financialPeriod.Id;
        result.FinancialPeriodNumber = financialPeriod.YearNumber;


        var lastEntryNumber =  _dbContext.Set<Entry>()
                                    .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                                    .ToList()
                                    .OrderByDescending(e =>BigInteger.Parse(e.EntryNumber))
                                    .Select(e =>BigInteger.Parse(e.EntryNumber))
                                    .FirstOrDefault();
        if (lastEntryNumber == null)
            lastEntryNumber = 0;
        
        result.EntryNumber = (lastEntryNumber + 1).ToString();
                
        return new ApiResponse<EntryNumberDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }


    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(EntryCreateCommand command)
    {
       var validationResult =  await base.ValidateCreate(command);
        
       var financialPeriod = await _dbContext.Set<FinancialPeriod>()
           .FirstOrDefaultAsync(e => e.StartDate <= command.EntryDate && e.EndDate > command.EntryDate);
       if (financialPeriod == null || command.FinancialPeriodId != financialPeriod.Id)
       {
           validationResult.isValid = false;
           validationResult.errors.Add("NotFoundCurrentFinancialPeriod");
       }

       var entryWithSameNumber = await _dbContext.Set<Entry>()
           .Where(e => e.EntryNumber == command.EntryNumber && e.FinancialPeriodId == command.FinancialPeriodId)
           .FirstOrDefaultAsync();
       if (entryWithSameNumber != null)
       {
           validationResult.isValid = false;
           validationResult.errors.Add("ExistedEntryWithSameNumber");
       }

       return validationResult;
    }

    protected override Task<(bool isValid, List<string> errors, Entry? entity)> ValidateUpdate(EntryUpdateCommand command)
    {
        return  base.ValidateUpdate(command);
    }
}