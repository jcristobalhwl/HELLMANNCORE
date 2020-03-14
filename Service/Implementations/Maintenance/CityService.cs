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

namespace Service.Implementations.Maintenance
{
    public class CityService : IServiceBase<TBL_SLI_CITY>
    {
        private readonly DbCore _context;
        private ResponseBase<TBL_SLI_CITY> _response;
        private ValidationResult _results;
        public CityService()
        {
            _context = new DbCore();
        }
        public ResponseBase<CityListResponse> GetList()
        {
            ResponseBase<CityListResponse> response;
            try
            {
                var query = _context.TBL_SLI_CITY.Where(x => x.BIT_STATE == true).Select(x => new CityListResponse
                {
                    INT_CITYID = x.INT_CITYID,
                    VCH_CITYNAME = x.VCH_NAME,
                    VCH_COUNTRYNAME = x.TBL_SLI_COUNTRY.VCH_NAME
                }).OrderBy(x => x.VCH_COUNTRYNAME);
                response = new UtilityResponse<CityListResponse>().SetResponseBaseForList(query);
                return response;
            }
            catch (Exception ex)
            {
                response = new UtilityResponse<CityListResponse>().SetResponseBaseForException(ex);
                return response;
            }
            finally
            {
                response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CITY> Get(int ID)
        {
            try
            {
                var cityFound = _context.TBL_SLI_CITY.Find(ID);
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForObj(cityFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CITY> Insert(TBL_SLI_CITY country)
        {
            CityValidator validator;
            try
            {
                validator = new CityValidator();
                _results = validator.Validate(country);
                if (_results.IsValid)
                {
                    country.BIT_STATE = true;
                    country.VCH_NAME = country.VCH_NAME.ToUpper();
                    var cityFound = _context.TBL_SLI_CITY.Where(x => x.VCH_NAME.Contains(country.VCH_NAME)).FirstOrDefault();
                    _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForUniqueValidation();
                    if (cityFound == null)
                    {
                        _context.TBL_SLI_CITY.Add(country);
                        _context.SaveChanges();
                        _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForObj(country);
                    }
                }
                else
                {
                    _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseFunctionalErrors(_results);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForException(ex);
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

        public ResponseBase<TBL_SLI_CITY> Update(TBL_SLI_CITY country)
        {
            try
            {
                var cityUpdated = _context.TBL_SLI_CITY.Find(country.INT_COUNTRYID);
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForObj(cityUpdated);
                if (cityUpdated != null)
                {
                    cityUpdated.VCH_NAME = country.VCH_NAME.ToUpper();
                    cityUpdated.VCH_INTCODE = country.VCH_INTCODE;
                    cityUpdated.INT_COUNTRYID = country.INT_COUNTRYID;
                    _context.SaveChanges();
                    _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForObj(cityUpdated);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CITY> Delete(int ID)
        {
            try
            {
                var cityFound = _context.TBL_SLI_CITY.Find(ID);
                cityFound.BIT_STATE = false;
                _context.SaveChanges();
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForObj(cityFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_CITY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        ResponseBase<TBL_SLI_CITY> IServiceBase<TBL_SLI_CITY>.GetList()
        {
            throw new NotImplementedException();
        }
    }
}
