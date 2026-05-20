using MedApp.Application.Common.DTOs;
using MedApp.Domain.Entities;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Specialties.Commands;

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, SpecialtyDto>
{
    private readonly IUnitOfWork _uow;

    public CreateSpecialtyCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SpecialtyDto> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new Specialty
        {
            Name = request.Name,
            Description = request.Description
        };

        await _uow.Specialties.AddAsync(specialty);
        await _uow.SaveChangesAsync();

        return new SpecialtyDto(specialty.Id, specialty.Name, specialty.Description);
    }
}