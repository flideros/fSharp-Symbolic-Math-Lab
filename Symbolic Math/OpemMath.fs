namespace OpenMath

open FSharp.Data

///OpenMath ///
type OpenMath = XmlProvider<Schema = "https://www.openmath.org/standard/om20-2017-07-22/openmath2.xsd">

///OpenMath Content Dictionary///
type OpenMathCD = XmlProvider<"omExample.ocd">

///OpenMath Content Dictionary Group///
type OpenMathCDGroup  = XmlProvider<"https://www.openmath.org/cdgroups/algstr1.cdg">

///OpenMath Content Dictionary Group///
type OpenMathCDSignature  = XmlProvider<"https://www.openmath.org/sts/arith1.sts">

///Content Dictionary Entry///
type CDDefinition = {Name:string;
                     Description:string;
                     Role:Option<string>;
                     Examples:OpenMathCD.Example[];
                     CMPs:string[];                 
                     FMPs:OpenMathCD.Fmp[]}


///Content Dictionary Group Member///
type CDGroupMember = {CDComment:string;
                      CDName: string;
                      CDUrl: string}


type CDSignature = {Name:string; Signature:XmlProvider<"https://www.openmath.org/sts/arith1.sts">.Omobj}



module GET = 
        
    let cD ocd = let d = (__SOURCE_DIRECTORY__ + @"\OCD\" + ocd + ".ocd")
                 OpenMathCD.Load(d)

    let cDFile (file : XmlProvider<"omExample.ocd">.Cd) = file.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\" + file.CdName + ".ocd")
    
    let private _cD ocd = let d = "http://www.openmath.org/cd/" + ocd + ".ocd"
                          OpenMathCD.Load(d)

    let cDFiles files = List.iter (fun x -> let file  = _cD x
                                            file.XElement.Save(__SOURCE_DIRECTORY__ + @"\OCD\"  + file.CdName + ".ocd") ) files

    let definitions ocd = (cD ocd).CdDefinitions |> Array.map (fun entry ->
                                                   {Name = entry.Name; 
                                                    Description = entry.Description; 
                                                    Role = entry.Role;
                                                    Examples = entry.Examples; 
                                                    CMPs = entry.Cmps; 
                                                    FMPs = entry.Fmps});

    let definitionEntry cd symbol = cd |> Array.tryFind (fun (r : CDDefinition) -> r.Name = symbol)
 
    let cDGroup cdg = let d = "http://www.openmath.org/cdgroups/" + cdg + ".cdg"
                      OpenMathCDGroup.Load(d)
                      

    let cDSignature sts = let d = "http://www.openmath.org/sts/" + sts + ".sts"
                          OpenMathCDSignature.Load(d)
