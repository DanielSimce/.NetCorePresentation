using Presentaiton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentaiton.Repository
{
    public interface INbaplayer
    {
        List<NbaPlayer> AllPlayers();
        NbaPlayer PlayerById(int id);
        void Create(NbaPlayer nbaPlayer);
        void Update(NbaPlayer nbaPlayer);
        void Delete(NbaPlayer nbaPlayer);

        void Save();


    }
}
