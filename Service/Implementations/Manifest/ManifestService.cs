using Common;
using Domain;
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
        private readonly DB_MANIFESTCONNECTION _context;

        public ManifestService()
        {
            _context = new DB_MANIFESTCONNECTION();
        }
        public void insertManifest(ref List<TBL_ADU_MANIFEST> manifestList)
        {
            try
            {
                _context.TBL_ADU_MANIFEST.AddRange(manifestList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }
        public void insertManifestShipmentDocument(ref List<TBL_ADU_MANIFESTSHIPMENTDOC> manifestSDList)
        {
            try
            {
                _context.TBL_ADU_MANIFESTSHIPMENTDOC.AddRange(manifestSDList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }
        public void insertManifestShipmentDetailDocument(ref List<TBL_ADU_MANIFESTSHIPMENTDETAILDOC> manifestSDDetailList)
        {
            try
            {
                _context.TBL_ADU_MANIFESTSHIPMENTDETAILDOC.AddRange(manifestSDDetailList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }
        public void insertAduanaDestinations(ref List<TBL_ADU_ADUANADESTINATION> aduanaDestinationList)
        {
            try
            {
                _context.TBL_ADU_ADUANADESTINATION.AddRange(aduanaDestinationList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }
        public void insertMasterInformation(ref List<TBL_ADU_MASTERINFORMATION> masterInformationList)
        {
            try
            {
                _context.TBL_ADU_MASTERINFORMATION.AddRange(masterInformationList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }
        public void insertWareDescription(ref List<TBL_ADU_WAREDESCRIPTION> manifestSDDetailList)
        {
            try
            {
                _context.TBL_ADU_WAREDESCRIPTION.AddRange(manifestSDDetailList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
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
                _context.Database.Connection.Close();
                throw ex;
            }
        }

        public void insertTracks(ref List<TBL_ADU_TRACK> trackList)
        {
            try
            {
                _context.TBL_ADU_TRACK.AddRange(trackList);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Database.Connection.Close();
                throw ex;
            }
        }

        public ResponseBase<TBL_MAN_MANIFEST> callStoreProcedureManifest()
        {
            ResponseBase<TBL_MAN_MANIFEST> response;
            try
            {
                var resultSP = _context.InsertAndGetManifests();

                response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForOK(resultSP.ToList());
                return response;
            }
            catch (Exception ex)
            {
                response = new UtilitariesResponse<TBL_MAN_MANIFEST>().SetResponseBaseForException(ex);
                _context.Database.Connection.Close();
                throw ex;
            }
        }
    }
}
