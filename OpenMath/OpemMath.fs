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
type CDEntry =  {Name:string;
                 Description:string;
                 Role:Option<string>;
                 Examples:OpenMathCD.Example[];
                 CMPs:string[];                 
                 FMPs:OpenMathCD.Fmp[];                 
                }

(*///Content Dictionary///
type CD = {Base:string;
           Comments:string[];
           Date:System.DateTime;
           Definitions:CDEntry[];
           Name:string;
           ReviewDate:System.DateTime;
           Revision:int;
           Status:string;
           Url:string;
           Version:int;
           Description:string}*)          

///Content Dictionary Group Member///
type CDGroupMember = {CDComment:string;
                      CDName: string;
                      CDUrl: string}

(*///Content Dictionary Group///
type CDGroup = {GroupDescription:string
                GroupMembers:CDGroupMember[]
                GroupName:string
                GroupRevision:int
                GroupUrl:string
                GroupVersion:int
                Version:decimal
               }*)

type CDSignature = {Name:string; Signature:XmlProvider<"https://www.openmath.org/sts/arith1.sts">.Omobj}

(*type CDSignatureFile = {CD:string;
                        Comment:string;
                        Status:string;
                        Signatures:CDSignature[];
                        Type:string;
                        Version:decimal;
                        Url:string
                       }*)

module GET = 
    
    let cD ocd = let d = "http://www.openmath.org/cd/" + ocd + ".ocd"
                 OpenMathCD.Load(d)
    
    let cDDefinitions (cd : OpenMathCD.Cd) = cd.CdDefinitions 
                                                |> Array.map (fun x -> 
                                                        {Name = x.Name;
                                                         Description = x.Description;
                                                         Role = x.Role;
                                                         Examples = x.Examples;
                                                         CMPs = x.Cmps;                 
                                                         FMPs = x.Fmps})
 
    let cDGroup cdg = let d = "http://www.openmath.org/cdgroups/" + cdg + ".cdg"
                      OpenMathCDGroup.Load(d)
                     

    let cDSignature sts = let d = "http://www.openmath.org/sts/" + sts + ".sts"
                          OpenMathCDSignature.Load(d)
                          