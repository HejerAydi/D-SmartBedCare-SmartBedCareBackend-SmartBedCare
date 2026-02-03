using Domain.Entities;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class testservices:ITestServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public testservices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<IReadOnlyList<Test>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Test>().GetAllAsyncwithfilter();
                IReadOnlyList<Test> readOnlyResult = result.ToList();
                return readOnlyResult;
            }
            catch (Exception ex) { 
            throw genException.GenericException.GenException(ex,_unitOfWork); 
            }
        }
        public async Task<Test?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Repository<Test>().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
        public async Task AddAsync(Test entity)
        //
        {
            try
            {    
                await _unitOfWork.Repository<Test>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
        public async Task<Test?> UpdateAsync(int id, Test entity, List<string> fieldsToUpdate)
        //
        {
            try
            {
                var existing = await _unitOfWork.Repository<Test>().GetByIdAsync(id);

                await _unitOfWork.Repository<Test>().UpdateGeneral(existing, entity, fieldsToUpdate);
                return existing;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Test>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception("Erreur 020420251305 : entity n'existe pas");

                await _unitOfWork.Repository<Test>().DeleteAsync(existing);
             
                return true;
            }
            catch (Exception ex)
            {
                throw genException.GenericException.GenException(ex, _unitOfWork);
            }
        }
      

    }
}
