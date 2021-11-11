using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Exception;

public class decodePolyline : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

         //string aaa = DecodePolylinePoints("riqvD`h`wHDfBH`A^fDDz@@rB?hFOnBU|A{@xEy@nEk@rEi@tC]lAy@zBmBpEuDtIs@pAcBfCeHnJaIlKqAnBu@vAaArC_F`TsAbFOj@}@pBu@`B_AbB_@n@MPO?KBUPo@^gAZU?_@Ai@MWKmCkBmB^oLlCcEdAw@Xq@Zu@^m@`@{@r@u@r@yAlB}@`Bk@nAiBxF}DtL{@tCYtAa@lEIjDAlIClLAfPo@HoDt@u@BCH?hJaBjAgBjBeA~@qAlAeCxBsBbAoCnAeClAYHw@J}ABu@IcEo@uD[cXcAwGY}FYa@EYOi@bAcAvBoBlDQR[T}CpAe@NiAt@w@v@}@~A?JBNQd@}@nBw@vAqAbBaA`AaGdE{JhH_Bl@mC`AmCz@uAZqAf@wFjBmK|Do@VuD|AmJvD}D~AaHtE}CrBeAXyJ`@gFX{EXuAd@eBp@aB|@s@`@gDrAuJfDaC|@qEnBaFrBk@Ve@Dy@CyAWg@EoFHmBHuHNc@?eAIqDo@}FsA_AbH{BjQkDdXcAlIQpA_@vCjB`@jB^DLHX?VUxAU`Bk@jDo@pDYnAUb@g@f@o@Zk@Pe@Xe@j@IZEd@JlAZjAJh@PjA?b@Ep@gEjR_EzQeEfQwBtJwI``@kObq@iDrOiKxd@gDfOm@xCi@bDe@fE]hGGrCAvBJ~FV`Ej@pF\\`Ct@pDjAxFbFdVdDdPvBtJfBhIjBtI|EtWfE~P~BnLvAdHzCxOd@fBnD`QhMxm@`Klf@rQp{@jDnPhBxElA|BrBpCjAjA|@r@vBtAnB|@nPhG|ZjLjLnE~YxKjLnEtb@`PtJxDlF|BhKpDxVhJjQvG|S|Hxc@vPnp@rVzTlIlHjC~A^hBXlDXnb@`B`h@nBn]tAfHX|DHvPj@tFT`YjAhmA|E~^pAfH\\pDZvBZ`Cn@jGjBlBn@dEnA~QlGjA^vAb@`EhAP`@Ld@B`@G^[d@]RU?a@I[QcA{AwAyBKWI[DkAFg@FQGYHUK]KMiA_@o@MIA]]Uc@i@qAk@yBc@yB[cDSuKC_CBG?EIKE}DWeGS}B]_CyAyHqEwUk@cBe@}@?IAGIOOWaBaE[mAU}AO{D?OIqBK_A[{A_@qA}@uBwAsBiDgDoCyCaEcEmDpEuAjB");

        //string bbb = DecodePolylinePoints("ffbvDzlowHMfAQfAYbBYvACPGb@Ib@ERERERGRA@OX?@CDCBCBABKJCBEBCBCBEBEBMHUHE@KDE@SFIDEBKFIFQPKJEHABCHADABAF?BADCN?J?H?H@F@F?LDT@@?B@F@DLb@HXBN@@DT?@BN@J@LH`@?P?PC^APCNcAnEmBtIQt@{@zDYhAERcB`Is@zCoAnFc@fB]rAu@lDS`Ac@jBIXm@vC_A~Dy@rDIXCNMf@kCnLGRETMf@s@|Ci@xBYtAc@lBc@nB[rA]vA]zAMh@i@dCqAzFOn@s@~CQt@Kd@GX[tAs@bDEPEPUdAc@nBk@dCMf@k@~BqA~Fs@~CuBlJS~@S|@y@rDc@nBo@pCYpAWfAUpAWzAQfAKt@MbAGl@C^I~@Ex@GfAEfAC~@CrAAbA?r@@|@@z@B~@BdAHnAFjA@TBNHdAD`@Hr@PtAFb@T|AVpA\\~ATbA^jBH^Jh@R|@P|@zCxN`@nBbCnL`@tBr@|CbAvE|AjHBJ@@BN|ArHDP@?DN|BfM~AlItA|FpB`IDTDTp@`D`A`FBJ?@BJnAjGzCxOd@fBdCnLh@pCr@jD~ArH|EzUvA|GlCpM|ApHvA`H|@fEjIz`@|@hEbBbIT~@bA`FBHBLBLBJNl@bC|LRv@Pj@Zt@Vn@b@fAj@hA`@r@z@lAv@bAHJ`A~@RPh@`@lAx@h@ZPH|Ar@lBt@`Cz@jAb@rFrBlAb@fCbAlMzExEfBlDtA|FxBbUpIzCfAzAj@lBv@D@zEhBJDjUtIdBn@vHtCbA^rGdCJFPJt@^vD|AfGtB`Cz@`AZtCdApEbBnIbD`EzAdE|A~Aj@bBp@HDVJjE~AdC|@rAf@bA\\n@V`Bn@THvBz@nCbAnBt@fAb@|Bz@tJpDnEdBnBr@dC~@nAf@\\LdUrIfFlBxDvAbBn@tEdB|DzAjDnAzBz@|Al@jAb@jA`@v@Vx@Rd@Jp@Lv@JXDXBv@F`AFnAF|AF`ADp@BbABbK`@jDNxEPfGTzDNhDN`ENtBHv@BjDNvCJjGVjHXhGVlCJlBFxDP|BH~@?pEPZ@lENzBFzBFj@Dj@D`@@^@b@Bd@@fCLxFVzAF`ABvDPhEPfWdAbZjArSx@|Uz@`HTfH\\pDZZDzAT|@VbAVnBh@zC`Aj@R`AZ|Ad@fBh@bMfEzCdAb@L@@XHJDfA\\ND`EhA");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private string DecodePolylinePoints(string encodedPoints)
    {
        if (encodedPoints == null || encodedPoints == "") return null;
        string poly = "";
        char[] polylinechars = encodedPoints.ToCharArray();
        int index = 0;

        int currentLat = 0;
        int currentLng = 0;
        int next5bits;
        int sum;
        int shifter;
        int counter = 0;

        try
        {
            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                double lat, lon;

                lat = (double)currentLat / 100000.0;
                lon = (double)currentLng / 100000.0;
                poly += "|" + lat.ToString() + "," + lon.ToString();
                counter++;  //cada polyline pode ter no máx 100 pontos pra ser desenhada
            }
        }
        catch (Exception ex)
        {
            // logo it
        }

        Debug.Log(poly);

        return poly;
    }

}