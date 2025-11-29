using Esg.Horta.Entities;
using Fiap.Web.Alunos.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Canteiro;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Service.Implementations
{
    public class CanteiroService : ICanteiroService
    {
        private readonly DatabaseContext _context;

        public CanteiroService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<CanteiroPagedView<CanteiroListItemView>> ListarAsync(int page, int pageSize)
        {
            var query = _context.Canteiros.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CanteiroListItemView
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    EspecieId = c.EspecieId,
                    AreaM2 = c.AreaM2,
                    MetaDoacaoKg = c.MetaDoacaoKg
                })
            .ToListAsync();

            return new CanteiroPagedView<CanteiroListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }


        // CRIAÇÃO
        public async Task<CanteiroListItemView> CriarAsync(CanteiroCreateView model)
        {
            // validações
            if (!await _context.Especies.AnyAsync(e => e.Id == model.EspecieId))
                throw new Exception("Espécie não encontrada");

            if (await _context.Canteiros.AnyAsync(c => c.Id == model.Id))
                throw new Exception("Id já existe — o ID NÃO é autogerado");

            var entity = new Canteiro
            {
                Id = model.Id,
                Nome = model.Nome,
                EspecieId = model.EspecieId,
                AreaM2 = model.AreaM2,
                MetaDoacaoKg = model.MetaDoacaoKg
            };

            _context.Canteiros.Add(entity);
            await _context.SaveChangesAsync();

            return new CanteiroListItemView
            {
                Id = entity.Id,
                Nome = entity.Nome,
                EspecieId = entity.EspecieId,
                AreaM2 = entity.AreaM2,
                MetaDoacaoKg = entity.MetaDoacaoKg
            };
        }
    }

}
