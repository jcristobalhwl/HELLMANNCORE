using Common;
using Domain;
using FluentValidation.Results;
using Model.Response;
using Service.Interfaces;
using Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class CountryService : IServiceBase<TBL_SLI_COUNTRY>
    {
        private readonly DbCore _context;
        private ResponseBase<TBL_SLI_COUNTRY> _response;
        private ValidationResult _results;
        public CountryService()
        {
            _context = new DbCore();
        }
        public ResponseBase<CountryListResponse> GetList()
        {
            ResponseBase<CountryListResponse> response;
            try
            {
                var query = _context.TBL_SLI_COUNTRY.Where(x => x.BIT_STATE == true).Select(x => new CountryListResponse
                {
                    INT_COUNTRYID = x.INT_COUNTRYID,
                    VCH_COUNTRYNAME = x.VCH_NAME,
                    VCH_CURRENCYNAME = x.TBL_SLI_CURRENCY.VCH_NAME,
                    VCH_FREIGHTCODE = x.VCH_FREIGHTCODE
                }).OrderBy(x => x.VCH_COUNTRYNAME);
                response = new UtilityResponse<CountryListResponse>().SetResponseBaseForList(query);
                return response;
            }
            catch (Exception ex)
            {
                response = new UtilityResponse<CountryListResponse>().SetResponseBaseForException(ex);
                return response;
            }
            finally
            {
                response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_COUNTRY> Get(int ID)
        {
            try
            {
                var countryFound = _context.TBL_SLI_COUNTRY.Find(ID);
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForObj(countryFound);
                return _response;
            }
            catch(Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_COUNTRY> Insert(TBL_SLI_COUNTRY country)
        {
            CountryValidator validator;
            try
            {
                validator = new CountryValidator();
                _results = validator.Validate(country);
                if (_results.IsValid)
                {
                    country.BIT_STATE = true;
                    country.VCH_NAME = country.VCH_NAME.ToUpper();
                    var countryFound = _context.TBL_SLI_COUNTRY.Where(x => x.VCH_NAME.Contains(country.VCH_NAME)).FirstOrDefault();
                    _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForUniqueValidation();
                    if (countryFound == null)
                    {
                        _context.TBL_SLI_COUNTRY.Add(country);
                        _context.SaveChanges();
                        _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForObj(country);
                    }
                }
                else
                {
                    _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseFunctionalErrors(_results);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForException(ex);
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

        public ResponseBase<TBL_SLI_COUNTRY> Update(TBL_SLI_COUNTRY country)
        {
            try
            {
                var countryUpdated = _context.TBL_SLI_COUNTRY.Find(country.INT_COUNTRYID);
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForObj(countryUpdated);
                if (countryUpdated != null)
                {
                    countryUpdated.VCH_NAME = country.VCH_NAME.ToUpper();
                    countryUpdated.VCH_FREIGHTCODE = country.VCH_FREIGHTCODE.ToUpper();
                    countryUpdated.INT_CONTINENTID = country.INT_CONTINENTID;
                    countryUpdated.INT_CURRENCYID = country.INT_CURRENCYID;
                    _context.SaveChanges();
                    _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForObj(countryUpdated);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_COUNTRY> Delete(int ID)
        {
            try
            {
                var countryFound = _context.TBL_SLI_COUNTRY.Find(ID);
                countryFound.BIT_STATE = false;
                _context.SaveChanges();
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForObj(countryFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_COUNTRY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        ResponseBase<TBL_SLI_COUNTRY> IServiceBase<TBL_SLI_COUNTRY>.GetList()
        {
            throw new NotImplementedException();
        }
    }
}
