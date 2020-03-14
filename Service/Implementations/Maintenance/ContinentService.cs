using Common;
using Domain;
using FluentValidation.Results;
using Service.Interfaces;
using Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.Maintenance
{
    public class ContinentService : IServiceBase<TBL_SLI_CONTINENT>
    {
        private readonly DbCore _context;
        private ResponseBase<TBL_SLI_CONTINENT> _response;
        private ValidationResult _results;
        public ContinentService()
        {
            _context = new DbCore();
        }

        public ResponseBase<TBL_SLI_CONTINENT> GetList()
        {
            try
            {
                var query = _context.TBL_SLI_CONTINENT.Where(x => x.BIT_STATE == true);
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForList(query);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CONTINENT> Get(int ID)
        {
            try
            {
                var continentFound = _context.TBL_SLI_CONTINENT.Find(ID);
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForObj(continentFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        public ResponseBase<TBL_SLI_CONTINENT> Insert(TBL_SLI_CONTINENT continent)
        {
            ContinentValidator validator;
            try
            {
                validator = new ContinentValidator();
                _results = validator.Validate(continent);
                if (_results.IsValid)
                {
                    continent.BIT_STATE = true;
                    continent.VCH_NAME = continent.VCH_NAME.ToUpper();
                    var continentFound = _context.TBL_SLI_CONTINENT.Where(x => x.VCH_NAME.Contains(continent.VCH_NAME)).FirstOrDefault();
                    _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForUniqueValidation();
                    if (continentFound == null)
                    {
                        _context.TBL_SLI_CONTINENT.Add(continent);
                        _context.SaveChanges();
                        _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForObj(continent);
                    }
                }
                else
                {
                    _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseFunctionalErrors(_results);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _results = null;
                validator = null;
                _context.Database.Connection.Close();
            }
        }

        public ResponseBase<TBL_SLI_CONTINENT> Update(TBL_SLI_CONTINENT continent)
        {
            try
            {
                var continentUpdated = _context.TBL_SLI_CONTINENT.Find(continent.INT_CONTINENTID);
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForObj(continentUpdated);
                if (continentUpdated != null)
                {
                    continentUpdated.VCH_NAME = continent.VCH_NAME.ToUpper();
                    _context.SaveChanges();
                    _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForObj(continentUpdated);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CONTINENT> Delete(int ID)
        {
            try
            {
                var continentFound = _context.TBL_SLI_CONTINENT.Find(ID);
                continentFound.BIT_STATE = false;
                _context.SaveChanges();
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForObj(continentFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CONTINENT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
    }
}
