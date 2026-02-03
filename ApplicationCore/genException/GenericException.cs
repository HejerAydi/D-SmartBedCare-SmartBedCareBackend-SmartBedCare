using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.genException
{
   public static class GenericException
    {
        public static Exception GenException(Exception ex, IUnitOfWork transaction)
        {
            string message = "";
            try
            {
                transaction?.RollbackTransaction();
            }
            catch (Exception ex2)
            {
                // Optionnel : Journaliser l'erreur si vous utilisez un logger
                // Console.WriteLine($"Erreur lors du rollback : {ex2.Message}");
            }

            // Extraire le message de l'exception
            if (ex.InnerException != null)
            {
                message = ex.InnerException.InnerException != null
                    ? ex.InnerException.InnerException.Message
                    : ex.InnerException.Message;
            }
            else
            {
                message = ex.Message;
            }

            // Nettoyer le message
            message = message?.Trim() ?? "Une erreur inconnue s'est produite.";

            // Diviser le message si un ':' est présent
            string[] element = message.Split(':');
            if (element.Length > 1)
            {
                string[] elem = element[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (elem.Length >= 3)
                {
                    // Construire un message formaté avec les trois premiers mots
                    message = $"{elem[0].Trim()} {elem[1].Trim()} {elem[2].Trim()} : {element[1].Trim()}";
                }
                // Si moins de 3 mots, garder le message original
            }

            return new Exception(message);
        }
    }
}
