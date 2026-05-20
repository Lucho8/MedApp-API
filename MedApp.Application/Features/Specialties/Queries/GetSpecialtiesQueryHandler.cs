using MedApp.Application.Common.DTOs;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Specialties.Queries;

public class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, List<SpecialtyDto>>
{
    private readonly IUnitOfWork _uow;

    public GetSpecialtiesQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<SpecialtyDto>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _uow.Specialties.FindAsync(s => s.IsActive);
        return specialties.Select(s => new SpecialtyDto(s.Id, s.Name, s.Description)).ToList();
    }
}