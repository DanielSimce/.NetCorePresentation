using Microsoft.AspNetCore.Mvc;
using Presentaiton.Models;
using Presentaiton.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace Presentaiton.Services
{
    public class NbaPlayerRepo:INbaplayer
    {
        private readonly DatabaseContext _context;
        public NbaPlayerRepo(DatabaseContext context)
        {
            _context = context;
        }

        public List<NbaPlayer> AllPlayers()
        {
            return _context.NbaPlayers.ToList();
        }

        public void Create(NbaPlayer nbaPlayer)
        {
            _context.NbaPlayers.Add(nbaPlayer);
        }

        public void Delete(NbaPlayer nbaPlayer)
        {
            _context.NbaPlayers.Remove(nbaPlayer);
        }

        public NbaPlayer PlayerById(int id)
        {
            return _context.NbaPlayers.FirstOrDefault(x => x.Id == id);


            
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(NbaPlayer nbaPlayer)
        {
            _context.NbaPlayers.Update(nbaPlayer);
        }
    }
}
