﻿using System;
using System.ComponentModel;

namespace G4_EmployeeRegister.Models
{
    public class FichajeModel : INotifyPropertyChanged
    {
        private int _idFichaje;
        private int _idUsuario;
        private DateTime _fechaHora;
        private string _tipo;
        private string _observaciones;

        public FichajeModel(int idFichaje, int idUsuario, DateTime fechaHora, string tipo, string observaciones)
        {
            IdFichaje = idFichaje;
            IdUsuario = idUsuario;
            FechaHora = fechaHora;
            Tipo = tipo;
            Observaciones = observaciones;
        }

        public int IdFichaje
        {
            get => _idFichaje;
            set
            {
                if (_idFichaje != value)
                {
                    _idFichaje = value;
                    OnPropertyChanged(nameof(IdFichaje));
                }
            }
        }

        public int IdUsuario
        {
            get => _idUsuario;
            set
            {
                if (_idUsuario != value)
                {
                    _idUsuario = value;
                    OnPropertyChanged(nameof(IdUsuario));
                }
            }
        }

        public DateTime FechaHora
        {
            get => _fechaHora;
            set
            {
                if (_fechaHora != value)
                {
                    _fechaHora = value;
                    OnPropertyChanged(nameof(FechaHora));
                }
            }
        }

        public string Tipo
        {
            get => _tipo;
            set
            {
                if (_tipo != value)
                {
                    _tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        public string Observaciones
        {
            get => _observaciones;
            set
            {
                if (_observaciones != value)
                {
                    _observaciones = value;
                    OnPropertyChanged(nameof(Observaciones));
                }
            }
        }

        // Evento para Notificar Cambios en las Propiedades.
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
