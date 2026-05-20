using MedApp.Application.Common.Exceptions;
using MedApp.Domain.Interfaces;
using MediatR;

namespace MedApp.Application.Features.Specialties.Commands;

public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteSpecialtyCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialties = await _uow.Specialties.FindAsync(s => s.Id == request.SpecialtyId);
        var specialty = specialties.FirstOrDefault()
            ?? throw new AppException("Especialidad no encontrada.", 404);

        specialty.IsActive = false;
        _uow.Specialties.Update(specialty);
        await _uow.SaveChangesAsync();
        return true;
    }
}