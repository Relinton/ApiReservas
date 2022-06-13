
using System.Collections.Generic;

namespace ApiReservas.Models.IRepository
{
    public interface IRepositoryReserva
    {
        IEnumerable<Reserva> Reservas { get; }
        Reserva this[int id] { get; }
        Reserva AddReserva(Reserva reserva);
        Reserva UpdateReserva(Reserva reserva);
        void DeleteReserva(int id);
    }
}
