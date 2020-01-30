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
    public class CurrencyService : IServiceBase<TBL_SLI_CURRENCY>
    {
        private readonly DbCore _context;
        private ResponseBase<TBL_SLI_CURRENCY> _response;
        private ValidationResult _results;
        public CurrencyService()
        {
            _context = new DbCore();
        }

        public ResponseBase<TBL_SLI_CURRENCY> GetList()
        {
            try
            {
                var query = _context.TBL_SLI_CURRENCY.Where(x => x.BIT_ACTIVE == true);
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForList(query);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CURRENCY> Get(int ID)
        {
            try
            {
                var currencyFound = _context.TBL_SLI_CURRENCY.Find(ID);
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForObj(currencyFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        public ResponseBase<TBL_SLI_CURRENCY> Insert(TBL_SLI_CURRENCY currency)
        {
            CurrencyValidator validator;
            try
            {
                validator = new CurrencyValidator();
                _results = validator.Validate(currency);
                if (_results.IsValid)
                {
                    currency.BIT_ACTIVE = true;
                    currency.VCH_NAME = currency.VCH_NAME.ToUpper();
                    currency.VCH_SYMBOL = currency.VCH_SYMBOL.ToUpper();
                    var currencyFound = _context.TBL_SLI_CURRENCY.Where(x => x.VCH_NAME.Contains(currency.VCH_NAME)).FirstOrDefault();
                    _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForUniqueValidation();
                    if (currencyFound == null)
                    {
                        _context.TBL_SLI_CURRENCY.Add(currency);
                        _context.SaveChanges();
                        _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForObj(currency);
                    }
                }
                else
                {
                    _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseFunctionalErrors(_results);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForException(ex);
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

        public ResponseBase<TBL_SLI_CURRENCY> Update(TBL_SLI_CURRENCY currency)
        {
            try
            {
                var currencyUpdated = _context.TBL_SLI_CURRENCY.Find(currency.INT_CURRENCYID);
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForObj(currencyUpdated);
                if (currencyUpdated != null)
                {
                    currencyUpdated.VCH_NAME = currency.VCH_NAME.ToUpper();
                    currencyUpdated.VCH_SYMBOL = currency.VCH_SYMBOL.ToUpper();

                    _context.SaveChanges();
                    _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForObj(currencyUpdated);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }
        public ResponseBase<TBL_SLI_CURRENCY> Delete(int ID)
        {
            try
            {
                var currencyFound = _context.TBL_SLI_CURRENCY.Find(ID);
                currencyFound.BIT_ACTIVE = false;
                _context.SaveChanges();
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForObj(currencyFound);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_SLI_CURRENCY>().SetResponseBaseForException(ex);
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
