using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Model.Features;
using Core.Application.Model.Features.Transfer;
using Core.Domain.Entity.SEIH;

namespace Infrastructure.Services.SEIH.Transfer;

public class TransferRequestNetworkService : ITransferRequestNetworkService
{
    private readonly ITransferRequestRepository _repository;

    public TransferRequestNetworkService(ITransferRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ReceiveAsync(TransferRequestNetworkDto dto)
    {
        var entity = new TransferRequestEntity
        {
            Id = dto.RequestId,
            IdHospitalFrom = dto.HospitalFromId,
            IdHospitalTo = dto.HospitalToId,
            InfoPatient = dto.InfoPatient,
            IdConsent = dto.ConsentId,
            RequestReason = dto.RequestReason,
            Status = TransferRequestStatus.Pending
        };

        return await _repository.CreateAsync(entity);
    }


    public async Task<IEnumerable<TransferRequestNetworkDto>> GetIncomingAsync(Guid hospitalId)
    {
        var entities = await _repository
            .GetHospitalRequestsAsync(hospitalId);

        return entities

            .Where(x => x.IdHospitalTo == hospitalId
         || x.IdHospitalFrom == hospitalId)
            .Select(x => new TransferRequestNetworkDto
            {
                RequestId = x.Id!.Value,
                HospitalFromId = x.IdHospitalFrom,
                HospitalToId = x.IdHospitalTo,
                InfoPatient = x.InfoPatient ?? "",
                ConsentId = x.IdConsent,
                CreatedAt = x.Created,
                Status = x.Status,
                TransferId = x.TransferId,
                RequestReason = x.RequestReason,
                ResponseReason = x.ResponseReason
            });
    }


    public async Task<bool> UpdateStatusAsync(TransferRequestResponseNetworkDto dto)
    {
        Console.WriteLine("===== HUB RECEIVE RESPONSE =====");
        Console.WriteLine("RequestId: " + dto.RequestId);
        Console.WriteLine("Status: " + dto.Status);
        Console.WriteLine("TransferId: " + dto.TransferId);
        Console.WriteLine("ResponseReason: " + dto.ResponseReason);
        var request = await _repository.GetByIdAsync(dto.RequestId);


        if (request == null)
            return false;

        if (request.Status != TransferRequestStatus.Pending && !dto.TransferId.HasValue)
        {
            return false;
        }

        request.Status = dto.Status;
        request.ResponseReason = dto.ResponseReason;
        if (dto.TransferId.HasValue)
        {
            request.TransferId = dto.TransferId;
        }

        await _repository.UpdateAsync(request);

        return true;
    }
}