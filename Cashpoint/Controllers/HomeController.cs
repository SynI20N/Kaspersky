using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cashpoint.Models;

namespace Cashpoint.Controllers;

public class HomeController : Controller
{
    private List<int> _nominals;
    private List<int> _nominalCounts;
    private Models.Cashpoint _cashPoint;
    public ActionResult Index()
    {
        initCashpoint();
        return View(_cashPoint);
    }

    [HttpPost]
    public ActionResult IsCashGiveable([FromBody] CashpointQuery query)
    {
        if(query.Cash == 0)
        {
            return PartialView("Error");
        }
        _nominals = new List<int>(query.TapeNominals);
        _nominalCounts = new List<int>(query.NominalsCount);
        try
        {
            int[] result = getTapeCounts(query.Cash);
            List<List<int>> concat = new List<List<int>>(2);
            for (int i = 0; i < result.Length; i++)
            {
                concat.Add(new List<int>() { result[i], _nominals[i] });
            }
            return PartialView("Tick", concat);
        }
        catch (Exception ex)
        {
            return PartialView("Error");
        }
    }

    private int[] getTapeCounts(int amount)
    {
        int n = _nominals.Count;
        int[] dp = new int[amount + 1];
        int[][] tapeCount = new int[amount + 1][];
        
        Array.Fill(dp, int.MaxValue);
        dp[0] = 0;
        
        for (int i = 0; i <= amount; i++)
        {
            tapeCount[i] = new int[n];
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = _nominals[i]; j <= amount; j++)
            {
                if (dp[j - _nominals[i]] != int.MaxValue 
                && dp[j - _nominals[i]] + 1 < dp[j] 
                && tapeCount[j - _nominals[i]][i] < _nominalCounts[i])
                {
                    dp[j] = dp[j - _nominals[i]] + 1;
                    Array.Copy(tapeCount[j - _nominals[i]], tapeCount[j], n);
                    tapeCount[j][i]++;
                }
            }
        }

        if (dp[amount] == int.MaxValue)
        {
            throw new InvalidOperationException("Cannot serve");
        }

        return tapeCount[amount];
    }

    private void initCashpoint()
    {
        _cashPoint = new Models.Cashpoint();
        _cashPoint.TapeCount = 3;
        _cashPoint.TapeNominals  = new List<int>() { 100, 500, 700 };
        _cashPoint.NominalsCount = new List<int>() { 2, 3, 4 };
        _cashPoint.TapeStatuses = new List<bool>() { true, true, true };
    }
}