public List<BandProduction> Get(string  CompanyId, string SiteId, string BandId)
    {
        SqlConnection SQLConnection;
        SqlDataAdapter adp;
        SQLConnection = new SqlConnection();
        SQLConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

        List<BandProduction> lstReturn = new List<BandProduction>();
            try
            {
           
            SQLConnection.Open();

                adp = new SqlDataAdapter("EXEC HOLBandWiseProdDataDaily '" + CompanyId.Trim() + "','" + SiteId.Trim() + "','" + BandId.Trim() + "'", SQLConnection);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                SQLConnection.Close();



                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BandProduction item = new BandProduction();

                    item.BandIdOFull = dr["BANDID"].ToString();
                    item.EfficiencyFull = Convert.ToDouble(dr["EFFICIENCY"].ToString());
                    item.TargetQtyFull = Convert.ToDecimal(dr["TARGETQTY"].ToString());
                    item.ActualQtyFll = Convert.ToDecimal(dr["ACTUALQTY"].ToString());
                    decimal gap = Convert.ToDecimal(dr["GAP"].ToString());
                    decimal min = Convert.ToDecimal(dr["BEHINDMIN"].ToString());
                    String gapstr, minstr;
                    if (gap < 0)
                    {

                        gapstr = '+' + Math.Abs(gap).ToString();
                        minstr = '+' + Math.Abs(min).ToString();

                    }
                    else
                    {
                        gapstr = gap.ToString();
                        minstr = min.ToString();
                    }
                    item.GapFull = gapstr;
                    item.WIPFull = Convert.ToDecimal(dr["WIP"].ToString());
                    item.StarFull = Convert.ToDecimal(dr["Star"].ToString());
                    item.minFull = minstr;
                    item.OverallRatioFull = Convert.ToDouble(dr["OVERALLRATIO"].ToString());
                    item.GAPColor = dr["GAPColor"].ToString();
                    item.CurrnrHourFull = dr["CURRENTHOUR"].ToString();
                    item.JobTimeOFull = dr["EXECUTEDDATETIME"].ToString();
                    lstReturn.Add(item);

                }

                return lstReturn;
            }
            catch (Exception ex)
            {
                if (SQLConnection.State != ConnectionState.Closed)
                {
                    SQLConnection.Close();
                }
                System.Web.Http.Results.ExceptionResult error = new System.Web.Http.Results.ExceptionResult(ex, this);
                BandProduction itemError = new BandProduction();
                itemError.BandIdOFull = ex.Message;
                lstReturn.Add(itemError);
                return lstReturn;
            }

    }

