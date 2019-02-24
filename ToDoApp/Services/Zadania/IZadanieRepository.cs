using System;
using System.Collections.Generic;
using ToDoApp.Areas.Zadania.Models;


namespace ToDoApp.Services
{
    public interface IZadanieRepository :IDisposable
    {
        IEnumerable<ToDoModel> GetAll();
        ToDoModel GetById(int id);
        int Add(ToDoModel item);
        int Edit(ToDoModel item);
        bool Delete(int id);
    }
}