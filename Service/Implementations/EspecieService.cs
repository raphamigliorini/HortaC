using Esg.Horta.Entities;
using Fiap.Web.Alunos.Data.Contexts;
using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Especie;
using FiapWebAluno.Views.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FiapWebAluno.Service.Implementations
{
    }
    public class EspecieService : IEspecieService
    {
        private readonly DatabaseContext _context;

        public EspecieService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PagedResultView<EspecieListItemView>> ListarAsync(int page, int pageSize)
        {
            var query = _context.Especies.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EspecieListItemView
                {
                    Id = e.Id,
                    Nome = e.Nome
                })
                .ToListAsync();

            return new PagedResultView<EspecieListItemView>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<EspecieListItemView> CriarAsync(EspecieCreateView model)
        {
            if (await _context.Especies.AnyAsync(x => x.Id == model.Id))
                throw new Exception("ID já existe.");

            var entity = new Especie
            {
                Id = model.Id,
                Nome = model.Nome
            };

            _context.Especies.Add(entity);
            await _context.SaveChangesAsync();

            return new EspecieListItemView
            {
                Id = entity.Id,
                Nome = entity.Nome
            };
        }
    }

