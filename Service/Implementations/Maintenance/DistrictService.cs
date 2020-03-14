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
    public class DistrictService : IServiceBase<TBL_SLI_DISTRICT>
    {
        private readonly DbCore _context;
        private ResponseBase<TBL_SLI_DISTRICT> _response;
        private ValidationResult _results;
        public DistrictService()
        {
            _context = new DbCore();
        }
        public ResponseBase<DistrictListResponse> GetList()
        {
            ResponseBase<DistrictListResponse> response;
            try
            {
                var query = _context.TBL_SLI_DISTRICT.Where(x => x.BIT_STATE == true).Select(x => new DistrictListResponse
                {
                    INT_DISTRICTID = x.INT_DISTRICTID,
                    VCH_DISTRICTNAME = x.VCH_NAME,
                    VCH_PROVINCENAME = x.TBL_SLI_PROVINCE.VCH_NAME,
                }).OrderBy(x => x.VCH_DISTRICTNAME);
                response = new UtilityResponse<DistrictListResponse>().SetResponseBaseForList(query);
                return response;
            }
            catch (Exception ex)
            {
                response = new UtilityResponse<DistrictListResponse>().SetResponseBaseForException(ex);
                return response;
            }
            finally
            {
                response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_DISTRICT> Get(int ID)
        {
            try
            {
                var countryFound = _context.TBL_SLI_DISTRICT.Find(ID);
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForObj(countryFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_DISTRICT> Insert(TBL_SLI_DISTRICT country)
        {
            DistrictValidator validator;
            try
            {
                validator = new DistrictValidator();
                _results = validator.Validate(country);
                if (_results.IsValid)
                {
                    country.BIT_STATE = true;
                    country.VCH_NAME = country.VCH_NAME.ToUpper();
                    var countryFound = _context.TBL_SLI_DISTRICT.Where(x => x.VCH_NAME.Contains(country.VCH_NAME)).FirstOrDefault();
                    _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForUniqueValidation();
                    if (countryFound == null)
                    {
                        _context.TBL_SLI_DISTRICT.Add(country);
                        _context.SaveChanges();
                        _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForObj(country);
                    }
                }
                else
                {
                    _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseFunctionalErrors(_results);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForException(ex);
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

        public ResponseBase<TBL_SLI_DISTRICT> Update(TBL_SLI_DISTRICT country)
        {
            try
            {
                var countryUpdated = _context.TBL_SLI_DISTRICT.Find(country.INT_DISTRICTID);
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForObj(countryUpdated);
                if (countryUpdated != null)
                {
                    countryUpdated.VCH_NAME = country.VCH_NAME.ToUpper();
                    countryUpdated.INT_PROVINCEID = country.INT_PROVINCEID;

                    _context.SaveChanges();
                    _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForObj(countryUpdated);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_DISTRICT> Delete(int ID)
        {
            try
            {
                var countryFound = _context.TBL_SLI_DISTRICT.Find(ID);
                countryFound.BIT_STATE = false;
                _context.SaveChanges();
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForObj(countryFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilityResponse<TBL_SLI_DISTRICT>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        ResponseBase<TBL_SLI_DISTRICT> IServiceBase<TBL_SLI_DISTRICT>.GetList()
        {
            throw new NotImplementedException();
        }
    }
}
