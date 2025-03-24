using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UomaWeb.Models;
using UomaWeb;
using UnityEngine;

namespace UomaWeb
{
    public partial class GameHelper : MonoBehaviour
    {
        private readonly GameWebApi _gameWebApi;

        public GameHelper()
        {
            _gameWebApi = new GameWebApi();
        }
    }
}