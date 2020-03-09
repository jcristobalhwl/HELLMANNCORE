using Common;
using Domain;
using Model.Request.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.Manifest
{
    public class ManifestService
    {
        private readonly DB_MANIFEST _context;
        private ResponseBase<TBL_MAN_MANIFEST> _response;
        public ManifestService()
        {
            _context = new DB_MANIFEST();
        }
        public void InsertManifest(ref List<TBL_ADU_MANIFEST> manifestList)
        {
            try
            {
                _context.TBL_ADU_MANIFEST.AddRange(manifestList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }
        public void InsertManifestShipmentDocument(ref List<TBL_ADU_MANIFESTSHIPMENTDOC> manifestSDList)
        {
            try
            {
                _context.TBL_ADU_MANIFESTSHIPMENTDOC.AddRange(manifestSDList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }
        public void InsertManifestShipmentDetailDocument(ref List<TBL_ADU_MANIFESTSHIPMENTDETAILDOC> manifestSDDetailList)
        {
            try
            {
                _context.TBL_ADU_MANIFESTSHIPMENTDETAILDOC.AddRange(manifestSDDetailList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }
        public void InsertAduanaDestinations(ref List<TBL_ADU_ADUANADESTINATION> aduanaDestinationList)
        {
            try
            {
                _context.TBL_ADU_ADUANADESTINATION.AddRange(aduanaDestinationList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }
        public void InsertMasterInformation(ref List<TBL_ADU_MASTERINFORMATION> masterInformationList)
        {
            try
            {
                _context.TBL_ADU_MASTERINFORMATION.AddRange(masterInformationList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }
        public void InsertWareDescription(ref List<TBL_ADU_WAREDESCRIPTION> manifestSDDetailList)
        {
            try
            {
                _context.TBL_ADU_WAREDESCRIPTION.AddRange(manifestSDDetailList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }

        public List<TBL_ADU_WEBTRACKING> getWebsTracking()
        {
            try
            {
               return _context.TBL_ADU_WEBTRACKING.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }

        public void InsertTracks(ref List<TBL_ADU_TRACK> trackList)
        {
            try
            {
                _context.TBL_ADU_TRACK.AddRange(trackList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _context.Database.Connection.Close();
            }
        }

        public ResponseBase<TBL_MAN_MANIFEST> CallStoreProcedureManifest()
        {
            try
            {
                var resultSP = _context.InsertAndGetManifests();

                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForOK(resultSP.ToList());
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _context.Database.Connection.Close();
                _response = null;
            }
        }

        public bool UpdateStateManifestDetail(TBL_ADU_MANIFESTSHIPMENTDETAILDOC manifestDetail)
        {
            var mDetailFound = _context.TBL_ADU_MANIFESTSHIPMENTDETAILDOC.Find(manifestDetail.DEC_MANIFESTSHIPDETDOCID);
            mDetailFound.BIT_COMPLETED = manifestDetail.BIT_COMPLETED;
            _context.SaveChanges();
            return true;
        }


        public ResponseBase<TBL_MAN_MANIFEST> GetManifests()
        {
            try
            {
                var queryResult = _context.TBL_MAN_MANIFEST.AsQueryable()
                //    .Where(x => x.DAT_DEPARTUREDATE >= request.DAT_STARTDATE && x.DAT_DEPARTUREDATE <= request.DAT_ENDDATE
                //&& (x.VCH_CONSIGNEE.Contains(request.VCH_CONSIGNEE) || x.VCH_SHIPPER.Contains(request.VCH_SHIPPER)
                //|| x.VCH_DESCRIPTION.Contains(request.VCH_DESCRIPTION) || x.VCH_AIRLINE.Contains(request.VCH_AIRLINE)
                //|| x.VCH_DESTINATION == request.VCH_DESTINATION || x.INT_WEEK == request.INT_WEEK))
                    ;

                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForList(queryResult);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForException(ex);
                return _response;
            }
            finally
            {
                _response = null;
                _context.Database.Connection.Close();
            }
        }

        public ResponseBase<TBL_MAN_MANIFEST> FindData(ManifestRequest request)
        {
            try
            {
                var queryResult = _context.TBL_MAN_MANIFEST.Where(x => x.DAT_DEPARTUREDATE >= request.DAT_STARTDATE && x.DAT_DEPARTUREDATE <= request.DAT_ENDDATE
                && (x.VCH_CONSIGNEE.Contains(request.VCH_CONSIGNEE) || x.VCH_SHIPPER.Contains(request.VCH_SHIPPER)
                || x.VCH_DESCRIPTION.Contains(request.VCH_DESCRIPTION) || x.VCH_AIRLINE.Contains(request.VCH_AIRLINE)
                || x.VCH_DESTINATION == request.VCH_DESTINATION || x.INT_WEEK == request.INT_WEEK));

                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForList(queryResult);
                return _response;
            }
            catch (Exception ex)
            {
                _response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForException(ex);
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
