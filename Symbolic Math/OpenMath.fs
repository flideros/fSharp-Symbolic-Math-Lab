namespace OpenMath

open System
open System.Xml.Linq
open FSharp.Data


///OpenMath ///
type OpenMath = XmlProvider<Schema = "https://www.openmath.org/standard/om20-2017-07-22/openmath2.xsd">

///OpenMath Content Dictionary///
type OpenMathCD = XmlProvider<Sample = "omExample.ocd">
///Content Dictionary Entry///
type CDDefinition = {Name:string;
                     Description:string;
                     Role:Option<string>;
                     Examples:OpenMathCD.Example[];
                     CMPs:string[];                 
                     FMPs:OpenMathCD.Fmp[]}

///OpenMath Content Dictionary Group///
type OpenMathCDGroup  = XmlProvider<Sample = "https://www.openmath.org/cdgroups/algstr1.cdg">
///Content Dictionary Group Member///
type CDGroupMember = {CDComment:string; CDName: string; CDUrl: string}

///OpenMath Content Dictionary Signature///
type OpenMathCDSignature  = XmlProvider<Sample = "https://www.openmath.org/sts/arith1.sts">
type CDSignature = {Name:string; Signature:XmlProvider<Sample = "https://www.openmath.org/sts/arith1.sts">.Omobj}

//module OpenMath =



[<RequireQualifiedAccess>]
module GET = 
    
    let cD ocd = 
        let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
        OpenMathCD.Load(d)

    let cDFile (file : XmlProvider<"omExample.ocd">.Cd) = 
        file.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\" + file.CdName + ".ocd")
    
    let private _cD ocd = let d = "http://www.openmath.org/cd/" + ocd + ".ocd"
                          OpenMathCD.Load(d)

    let cDFiles files = 
        List.iter (fun x -> let file  = _cD x
                            file.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\"  + file.CdName + ".ocd") ) files

    let definitions ocd = 
        let x = cD ocd
        x.CdDefinitions 
        |> Array.map (fun entry ->
            {Name = entry.Name; 
             Description = entry.Description; 
             Role = entry.Role;
             Examples = entry.Examples; 
             CMPs = entry.Cmps; 
             FMPs = entry.Fmps});

    let definitionEntry symbol cd = cd |> Array.tryFind (fun (r : CDDefinition) -> r.Name = symbol)
 
    let cDGroup cdg = let d = "http://www.openmath.org/cdgroups/" + cdg + ".cdg"
                      OpenMathCDGroup.Load(d)

    let cDSignature sts = let d = "http://www.openmath.org/sts/" + sts + ".sts"
                          OpenMathCDSignature.Load(d)

[<RequireQualifiedAccess>]
module FROM =

    let cD name = GET.definitions name