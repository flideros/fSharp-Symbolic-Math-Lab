namespace Math.Presentation

/// These encodings are derived from the STIX2 Math font implementation of the
/// Mathematical Alphanumeric Symbols Unicode block comprising styled forms of 
/// Latin and Greek letters and decimal digits that enable mathematicians to 
/// denote different notions with different letter styles. The letters in various 
/// fonts often have specific, fixed meanings in particular areas of mathematics. 
/// By providing uniformity over numerous mathematical articles and books, 
/// these conventions help to read mathematical formulae.
module MathematicalAlphanumericSymbols =

    // Use capital 'U' and the leadding zeroes to get the
    // unicode code points over 10000.
    module LatinSerif =         
        module Normal =
            let a,A = "\u0061", "\u0041"
            let b,B = "\u0062", "\u0042"
            let c,C = "\u0063", "\u0043"
            let d,D = "\u0064", "\u0044"
            let e,E = "\u0065", "\u0045"
            let f,F = "\u0066", "\u0046"
            let g,G = "\u0067", "\u0047"
            let h,H = "\u0068", "\u0048"
            let i,I = "\u0069", "\u0049"
            let iDotless = "\u0131"
            let jDotless = "\u0237"
            let j,J = "\u006a", "\u004a"
            let k,K = "\u006b", "\u004b"
            let l,L = "\u006c", "\u004c"
            let m,M = "\u006d", "\u004d"
            let n,N = "\u006e", "\u004e"
            let o,O = "\u006f", "\u004f"
            let p,P = "\u0070", "\u0050"
            let q,Q = "\u0071", "\u0051"
            let r,R = "\u0072", "\u0052"
            let s,S = "\u0073", "\u0053"
            let t,T = "\u0074", "\u0054"
            let u,U = "\u0075", "\u0055"
            let v,V = "\u0076", "\u0056"
            let w,W = "\u0077", "\u0057"
            let x,X = "\u0078", "\u0058"
            let y,Y = "\u0079", "\u0059"
            let z,Z = "\u007a", "\u005a"
        module Italic =
            let a,A = "\U0001d44e", "\U0001d434"
            let b,B = "\U0001d44f", "\U0001d435"
            let c,C = "\U0001d450", "\U0001d436"
            let d,D = "\U0001d451", "\U0001d437"
            let e,E = "\U0001d452", "\U0001d438"
            let f,F = "\U0001d453", "\U0001d439"
            let g,G = "\U0001d454", "\U0001d43A"
            let h,H = "\u210e"    , "\U0001d43B"
            let i,I = "\U0001d456", "\U0001d43C"
            let iDotless = "\U0001d6A4"
            let jDotless = "\U0001d6A5"
            let j,J = "\U0001d457", "\U0001d43D"
            let k,K = "\U0001d458", "\U0001d43E"
            let l,L = "\U0001d459", "\U0001d43F"
            let m,M = "\U0001d45a", "\U0001d440"
            let n,N = "\U0001d45b", "\U0001d441"
            let o,O = "\U0001d45c", "\U0001d442"
            let p,P = "\U0001d45d", "\U0001d443"
            let q,Q = "\U0001d45e", "\U0001d444"
            let r,R = "\U0001d45f", "\U0001d445"
            let s,S = "\U0001d460", "\U0001d446"
            let t,T = "\U0001d461", "\U0001d447"
            let u,U = "\U0001d462", "\U0001d448"
            let v,V = "\U0001d463", "\U0001d449"
            let w,W = "\U0001d464", "\U0001d44a"
            let x,X = "\U0001d465", "\U0001d44b"
            let y,Y = "\U0001d466", "\U0001d44c"
            let z,Z = "\U0001d467", "\U0001d44d"
        module Bold =
            let a,A = "\U0001D41A", "\U0001D400"
            let b,B = "\U0001D41B", "\U0001D401"
            let c,C = "\U0001D41C", "\U0001D402"
            let d,D = "\U0001D41D", "\U0001D403"
            let e,E = "\U0001D41E", "\U0001D404"
            let f,F = "\U0001D41F", "\U0001D405"
            let g,G = "\U0001D420", "\U0001D406"
            let h,H = "\U0001D421", "\U0001D407"
            let i,I = "\U0001D422", "\U0001D408"
            let j,J = "\U0001D423", "\U0001D409"
            let k,K = "\U0001D424", "\U0001D40A"
            let l,L = "\U0001D425", "\U0001D40B"
            let m,M = "\U0001D426", "\U0001D40C"
            let n,N = "\U0001D427", "\U0001D40D"
            let o,O = "\U0001D428", "\U0001D40E"
            let p,P = "\U0001D429", "\U0001D40F"
            let q,Q = "\U0001D42A", "\U0001D410"
            let r,R = "\U0001D42B", "\U0001D411"
            let s,S = "\U0001D42C", "\U0001D412"
            let t,T = "\U0001D42D", "\U0001D413"
            let u,U = "\U0001D42E", "\U0001D414"
            let v,V = "\U0001D42F", "\U0001D415"
            let w,W = "\U0001D430", "\U0001D416"
            let x,X = "\U0001D431", "\U0001D417"
            let y,Y = "\U0001D432", "\U0001D418"
            let z,Z = "\U0001D433", "\U0001D419"
        module BoldItalic =
            let a,A = "\U0001D482", "\U0001D468"
            let b,B = "\U0001D483", "\U0001D469"
            let c,C = "\U0001D484", "\U0001D46A"
            let d,D = "\U0001D485", "\U0001D46B"
            let e,E = "\U0001D486", "\U0001D46C"
            let f,F = "\U0001D487", "\U0001D46D"
            let g,G = "\U0001D488", "\U0001D46E"
            let h,H = "\U0001D489", "\U0001D46F"
            let i,I = "\U0001D48A", "\U0001D470"
            let j,J = "\U0001D48B", "\U0001D471"
            let k,K = "\U0001D48C", "\U0001D472"
            let l,L = "\U0001D48D", "\U0001D473"
            let m,M = "\U0001D48E", "\U0001D474"
            let n,N = "\U0001D48F", "\U0001D475"
            let o,O = "\U0001D490", "\U0001D476"
            let p,P = "\U0001D491", "\U0001D477"
            let q,Q = "\U0001D492", "\U0001D478"
            let r,R = "\U0001D493", "\U0001D479"
            let s,S = "\U0001D494", "\U0001D47A"
            let t,T = "\U0001D495", "\U0001D47B"
            let u,U = "\U0001D496", "\U0001D47C"
            let v,V = "\U0001D497", "\U0001D47D"
            let w,W = "\U0001D498", "\U0001D47E"
            let x,X = "\U0001D499", "\U0001D47F"
            let y,Y = "\U0001D49A", "\U0001D480"
            let z,Z = "\U0001D49B", "\U0001D481"

    module LatinSansSerif =         
        module Normal =
            let a,A = "\U0001D5BA", "\U0001D5A0"
            let b,B = "\U0001D5BB", "\U0001D5A1"
            let c,C = "\U0001D5BC", "\U0001D5A2"
            let d,D = "\U0001D5BD", "\U0001D5A3"
            let e,E = "\U0001D5BE", "\U0001D5A4"
            let f,F = "\U0001D5BF", "\U0001D5A5"
            let g,G = "\U0001D5C0", "\U0001D5A6"
            let h,H = "\U0001D5C1", "\U0001D5A7"
            let i,I = "\U0001D5C2", "\U0001D5A8"
            let j,J = "\U0001D5C3", "\U0001D5A9"
            let k,K = "\U0001D5C4", "\U0001D5AA"
            let l,L = "\U0001D5C5", "\U0001D5AB"
            let m,M = "\U0001D5C6", "\U0001D5AC"
            let n,N = "\U0001D5C7", "\U0001D5AD"
            let o,O = "\U0001D5C8", "\U0001D5AE"
            let p,P = "\U0001D5C9", "\U0001D5AF"
            let q,Q = "\U0001D5CA", "\U0001D5B0"
            let r,R = "\U0001D5CB", "\U0001D5B1"
            let s,S = "\U0001D5CC", "\U0001D5B2"
            let t,T = "\U0001D5CD", "\U0001D5B3"
            let u,U = "\U0001D5CE", "\U0001D5B4"
            let v,V = "\U0001D5CF", "\U0001D5B5"
            let w,W = "\U0001D5D0", "\U0001D5B6"
            let x,X = "\U0001D5D1", "\U0001D5B7"
            let y,Y = "\U0001D5D2", "\U0001D5B8"
            let z,Z = "\U0001D5D3", "\U0001D5B9"
        module Italic =
            let a,A = "\U0001D622", "\U0001D608"
            let b,B = "\U0001D623", "\U0001D609"
            let c,C = "\U0001D624", "\U0001D60A"
            let d,D = "\U0001D625", "\U0001D60B"
            let e,E = "\U0001D626", "\U0001D60C"
            let f,F = "\U0001D627", "\U0001D60D"
            let g,G = "\U0001D628", "\U0001D60E"
            let h,H = "\U0001D629", "\U0001D60F"
            let i,I = "\U0001D62A", "\U0001D610"
            let j,J = "\U0001D62B", "\U0001D611"
            let k,K = "\U0001D62C", "\U0001D612"
            let l,L = "\U0001D62D", "\U0001D613"
            let m,M = "\U0001D62E", "\U0001D614"
            let n,N = "\U0001D62F", "\U0001D615"
            let o,O = "\U0001D630", "\U0001D616"
            let p,P = "\U0001D631", "\U0001D617"
            let q,Q = "\U0001D632", "\U0001D618"
            let r,R = "\U0001D633", "\U0001D619"
            let s,S = "\U0001D634", "\U0001D61A"
            let t,T = "\U0001D635", "\U0001D61B"
            let u,U = "\U0001D636", "\U0001D61C"
            let v,V = "\U0001D637", "\U0001D61D"
            let w,W = "\U0001D638", "\U0001D61E"
            let x,X = "\U0001D639", "\U0001D61F"
            let y,Y = "\U0001D63A", "\U0001D620"
            let z,Z = "\U0001D63B", "\U0001D621"
        module Bold =
            let a,A = "\U0001D5EE", "\U0001D5D4"
            let b,B = "\U0001D5EF", "\U0001D5D5"
            let c,C = "\U0001D5F0", "\U0001D5D6"
            let d,D = "\U0001D5F1", "\U0001D5D7"
            let e,E = "\U0001D5F2", "\U0001D5D8"
            let f,F = "\U0001D5F3", "\U0001D5D9"
            let g,G = "\U0001D5F4", "\U0001D5DA"
            let h,H = "\U0001D5F5", "\U0001D5DB"
            let i,I = "\U0001D5F6", "\U0001D5DC"
            let j,J = "\U0001D5F7", "\U0001D5DD"
            let k,K = "\U0001D5F8", "\U0001D5DE"
            let l,L = "\U0001D5F9", "\U0001D5DF"
            let m,M = "\U0001D5FA", "\U0001D5E0"
            let n,N = "\U0001D5FB", "\U0001D5E1"
            let o,O = "\U0001D5FC", "\U0001D5E2"
            let p,P = "\U0001D5FD", "\U0001D5E3"
            let q,Q = "\U0001D5FE", "\U0001D5E4"
            let r,R = "\U0001D5FF", "\U0001D5E5"
            let s,S = "\U0001D600", "\U0001D5E6"
            let t,T = "\U0001D601", "\U0001D5E7"
            let u,U = "\U0001D602", "\U0001D5E8"
            let v,V = "\U0001D603", "\U0001D5E9"
            let w,W = "\U0001D604", "\U0001D5EA"
            let x,X = "\U0001D605", "\U0001D5EB"
            let y,Y = "\U0001D606", "\U0001D5EC"
            let z,Z = "\U0001D607", "\U0001D5ED"        
        module BoldItalic =
            let a,A = "\U0001D656", "\U0001D63C"
            let b,B = "\U0001D657", "\U0001D63D"
            let c,C = "\U0001D658", "\U0001D63E"
            let d,D = "\U0001D659", "\U0001D63F"
            let e,E = "\U0001D65A", "\U0001D640"
            let f,F = "\U0001D65B", "\U0001D641"
            let g,G = "\U0001D65C", "\U0001D642"
            let h,H = "\U0001D65D", "\U0001D643"
            let i,I = "\U0001D65E", "\U0001D644"
            let j,J = "\U0001D65F", "\U0001D645"
            let k,K = "\U0001D660", "\U0001D646"
            let l,L = "\U0001D661", "\U0001D647"
            let m,M = "\U0001D662", "\U0001D648"
            let n,N = "\U0001D663", "\U0001D649"
            let o,O = "\U0001D664", "\U0001D64A"
            let p,P = "\U0001D665", "\U0001D64B"
            let q,Q = "\U0001D666", "\U0001D64C"
            let r,R = "\U0001D667", "\U0001D64D"
            let s,S = "\U0001D668", "\U0001D64E"
            let t,T = "\U0001D669", "\U0001D64F"
            let u,U = "\U0001D66A", "\U0001D650"
            let v,V = "\U0001D66B", "\U0001D651"
            let w,W = "\U0001D66C", "\U0001D652"
            let x,X = "\U0001D66D", "\U0001D653"
            let y,Y = "\U0001D66E", "\U0001D654"
            let z,Z = "\U0001D66F", "\U0001D655"
            
    module LatinScript =         
        module Normal =
            let a,A = "\U0001D4B6", "\U0001D49C"
            let b,B = "\U0001D4B7", "\u212c"
            let c,C = "\U0001D4B8", "\U0001D49E"
            let d,D = "\U0001D4B9", "\U0001D49F"
            let e,E = "\u212f", "\u2130"
            let f,F = "\U0001D4BB", "\u2131"
            let g,G = "\u210a", "\U0001D4A2"
            let h,H = "\U0001D4BD", "\u210b"
            let i,I = "\U0001D4BE", "\u2110"
            let j,J = "\U0001D4BF", "\U0001D4A5"
            let k,K = "\U0001D4C0", "\U0001D4A6"
            let l,L = "\U0001D4C1", "\u2112"
            let m,M = "\U0001D4C2", "\u2133"
            let n,N = "\U0001D4C3", "\U0001D4A9"
            let o,O = "\u2134", "\U0001D4AA"
            let p,P = "\U0001D4C5", "\U0001D4AB"
            let q,Q = "\U0001D4C6", "\U0001D4AC"
            let r,R = "\U0001D4C7", "\u211b"
            let s,S = "\U0001D4C8", "\U0001D4AE"
            let t,T = "\U0001D4C9", "\U0001D4AF"
            let u,U = "\U0001D4CA", "\U0001D4B0"
            let v,V = "\U0001D4CB", "\U0001D4B1"
            let w,W = "\U0001D4CC", "\U0001D4B2"
            let x,X = "\U0001D4CD", "\U0001D4B3"
            let y,Y = "\U0001D4CE", "\U0001D4B4"
            let z,Z = "\U0001D4CF", "\U0001D4B5"
        module Bold = 
            let a,A = "\U0001D4EA", "\U0001D4D0"
            let b,B = "\U0001D4EB", "\U0001D4D1"
            let c,C = "\U0001D4EC", "\U0001D4D2"
            let d,D = "\U0001D4ED", "\U0001D4D3"
            let e,E = "\U0001D4EE", "\U0001D4D4"
            let f,F = "\U0001D4EF", "\U0001D4D5"
            let g,G = "\U0001D4F0", "\U0001D4D6"
            let h,H = "\U0001D4F1", "\U0001D4D7"
            let i,I = "\U0001D4F2", "\U0001D4D8"
            let j,J = "\U0001D4F3", "\U0001D4D9"
            let k,K = "\U0001D4F4", "\U0001D4DA"
            let l,L = "\U0001D4F5", "\U0001D4DB"
            let m,M = "\U0001D4F6", "\U0001D4DC"
            let n,N = "\U0001D4F7", "\U0001D4DD"
            let o,O = "\U0001D4F8", "\U0001D4DE"
            let p,P = "\U0001D4F9", "\U0001D4DF"
            let q,Q = "\U0001D4FA", "\U0001D4E0"
            let r,R = "\U0001D4FB", "\U0001D4E1"
            let s,S = "\U0001D4FC", "\U0001D4E2"
            let t,T = "\U0001D4FD", "\U0001D4E3"
            let u,U = "\U0001D4FE", "\U0001D4E4"
            let v,V = "\U0001D4FF", "\U0001D4E5"
            let w,W = "\U0001D500", "\U0001D4E6"
            let x,X = "\U0001D501", "\U0001D4E7"
            let y,Y = "\U0001D502", "\U0001D4E8"
            let z,Z = "\U0001D503", "\U0001D4E9"
            
    module Fraktur =         
        module Normal =
            let a,A = "\U0001D51E", "\U0001D504"
            let b,B = "\U0001D51F", "\U0001D505"
            let c,C = "\U0001D520", "\u212D"
            let d,D = "\U0001D521", "\U0001D507"
            let e,E = "\U0001D522", "\U0001D508"
            let f,F = "\U0001D523", "\U0001D509"
            let g,G = "\U0001D524", "\U0001D50A"
            let h,H = "\U0001D525", "\u210C"
            let i,I = "\U0001D526", "\u2111"
            let j,J = "\U0001D527", "\U0001D50D"
            let k,K = "\U0001D528", "\U0001D50E"
            let l,L = "\U0001D529", "\U0001D50F"
            let m,M = "\U0001D52A", "\U0001D510"
            let n,N = "\U0001D52B", "\U0001D511"
            let o,O = "\U0001D52C", "\U0001D512"
            let p,P = "\U0001D52D", "\U0001D513"
            let q,Q = "\U0001D52E", "\U0001D514"
            let r,R = "\U0001D52F", "\u211C"
            let s,S = "\U0001D530", "\U0001D516"
            let t,T = "\U0001D531", "\U0001D517"
            let u,U = "\U0001D532", "\U0001D518"
            let v,V = "\U0001D533", "\U0001D519"
            let w,W = "\U0001D534", "\U0001D51A"
            let x,X = "\U0001D535", "\U0001D51B"
            let y,Y = "\U0001D536", "\U0001D51C"
            let z,Z = "\U0001D537", "\u2128"            
        module Bold = 
            let a,A = "\U0001D586", "\U0001D56C"
            let b,B = "\U0001D587", "\U0001D56D"
            let c,C = "\U0001D588", "\U0001D56E"
            let d,D = "\U0001D589", "\U0001D56F"
            let e,E = "\U0001D58A", "\U0001D570"
            let f,F = "\U0001D58B", "\U0001D571"
            let g,G = "\U0001D58C", "\U0001D572"
            let h,H = "\U0001D58D", "\U0001D573"
            let i,I = "\U0001D58E", "\U0001D574"
            let j,J = "\U0001D58F", "\U0001D575"
            let k,K = "\U0001D590", "\U0001D576"
            let l,L = "\U0001D591", "\U0001D577"
            let m,M = "\U0001D592", "\U0001D578"
            let n,N = "\U0001D593", "\U0001D579"
            let o,O = "\U0001D594", "\U0001D57A"
            let p,P = "\U0001D595", "\U0001D57B"
            let q,Q = "\U0001D596", "\U0001D57C"
            let r,R = "\U0001D597", "\U0001D57D"
            let s,S = "\U0001D598", "\U0001D57E"
            let t,T = "\U0001D599", "\U0001D57F"
            let u,U = "\U0001D59A", "\U0001D580"
            let v,V = "\U0001D59B", "\U0001D581"
            let w,W = "\U0001D59C", "\U0001D582"
            let x,X = "\U0001D59D", "\U0001D583"
            let y,Y = "\U0001D59E", "\U0001D584"
            let z,Z = "\U0001D59F", "\U0001D585"
            
    module DoubleStruck =         
        let a,A = "\U0001D552", "\U0001D538"
        let b,B = "\U0001D553", "\U0001D539"
        let c,C = "\U0001D554", "\u2102"
        let d,D = "\U0001D555", "\U0001D53B"
        let e,E = "\U0001D556", "\U0001D53C"
        let f,F = "\U0001D557", "\U0001D53D"
        let g,G = "\U0001D558", "\U0001D53E"
        let h,H = "\U0001D559", "\u210D"
        let i,I = "\U0001D55A", "\U0001D540"
        let j,J = "\U0001D55B", "\U0001D541"
        let k,K = "\U0001D55C", "\U0001D542"
        let l,L = "\U0001D55D", "\U0001D543"
        let m,M = "\U0001D55E", "\U0001D544"
        let n,N = "\U0001D55F", "\u2115"
        let o,O = "\U0001D560", "\U0001D546"
        let p,P = "\U0001D561", "\u2119"
        let q,Q = "\U0001D562", "\u211A"
        let r,R = "\U0001D563", "\u211D"
        let s,S = "\U0001D564", "\U0001D54A"
        let t,T = "\U0001D565", "\U0001D54B"
        let u,U = "\U0001D566", "\U0001D54C"
        let v,V = "\U0001D567", "\U0001D54D"
        let w,W = "\U0001D568", "\U0001D54E"
        let x,X = "\U0001D569", "\U0001D54F"
        let y,Y = "\U0001D56A", "\U0001D550"
        let z,Z = "\U0001D56B", "\u2124"
        
    module MonoSpaced =         
        let a,A = "\U0001D68A", "\U0001D670"
        let b,B = "\U0001D68B", "\U0001D671"
        let c,C = "\U0001D68C", "\U0001D672"
        let d,D = "\U0001D68D", "\U0001D673"
        let e,E = "\U0001D68E", "\U0001D674"
        let f,F = "\U0001D68F", "\U0001D675"
        let g,G = "\U0001D690", "\U0001D676"
        let h,H = "\U0001D691", "\U0001D677"
        let i,I = "\U0001D692", "\U0001D678"
        let j,J = "\U0001D693", "\U0001D679"
        let k,K = "\U0001D694", "\U0001D67A"
        let l,L = "\U0001D695", "\U0001D67B"
        let m,M = "\U0001D696", "\U0001D67C"
        let n,N = "\U0001D697", "\U0001D67D"
        let o,O = "\U0001D698", "\U0001D67E"
        let p,P = "\U0001D699", "\U0001D67F"
        let q,Q = "\U0001D69A", "\U0001D680"
        let r,R = "\U0001D69B", "\U0001D681"
        let s,S = "\U0001D69C", "\U0001D682"
        let t,T = "\U0001D69D", "\U0001D683"
        let u,U = "\U0001D69E", "\U0001D684"
        let v,V = "\U0001D69F", "\U0001D685"
        let w,W = "\U0001D6A0", "\U0001D686"
        let x,X = "\U0001D6A1", "\U0001D687"
        let y,Y = "\U0001D6A2", "\U0001D688"
        let z,Z = "\U0001D6A3", "\U0001D689"
    
    module Greek = 
        module Capital =
            module Normal =
                let alpha = "\u0391"
                let beta = "\u0392"
                let gamma = "\u0393"
                let delta = "\u0394"
                let epsilon = "\u0395"
                let zeta = "\u0396"
                let eta = "\u0397"
                let theta = "\u0398"
                let iota = "\u0399"
                let kappa = "\u039A"
                let lamda = "\u039B"
                let mu = "\u039C"
                let nu = "\u039D"
                let xi = "\u039E"
                let omicron = "\u039F"
                let pi = "\u03A0"
                let rho = "\u03A1"
                let thetaSymbol = "\u03F4"
                let sigma = "\u03A3"
                let tau = "\u03A4"
                let upsilon = "\u03A5"
                let phi = "\u03A6"
                let chi = "\u03A7"
                let psi = "\u03A8"
                let omega = "\u03A9"
                let nabla = "\u2207"
                let digamma = "\u03DC"
            module Bold =
                let alpha = "\U0001D6A8"
                let beta = "\U0001D6A9"
                let gamma = "\U0001D6AA"
                let delta = "\U0001D6AB"
                let epsilon = "\U0001D6AC"
                let zeta = "\U0001D6AD"
                let eta = "\U0001D6AE"
                let theta = "\U0001D6AF"
                let iota = "\U0001D6B0"
                let kappa = "\U0001D6B1"
                let lamda = "\U0001D6B2"
                let mu = "\U0001D6B3"
                let nu = "\U0001D6B4"
                let xi = "\U0001D6B5"
                let omicron = "\U0001D6B6"
                let pi = "\U0001D6B7"
                let rho = "\U0001D6B8"
                let thetaSymbol = "\U0001D6B9"
                let sigma = "\U0001D6BA"
                let tau = "\U0001D6BB"
                let upsilon = "\U0001D6BC"
                let phi = "\U0001D6BD"
                let chi = "\U0001D6BE"
                let psi = "\U0001D6BF"
                let omega = "\U0001D6C0"
                let nabla = "\U0001D6C1"
                let digamma = "\U0001D7CA"
            module Italic =
                let alpha = "\U0001D6E2"
                let beta = "\U0001D6E3"
                let gamma = "\U0001D6E4"
                let delta = "\U0001D6E5"
                let epsilon = "\U0001D6E6"
                let zeta = "\U0001D6E7"
                let eta = "\U0001D6E8"
                let theta = "\U0001D6E9"
                let iota = "\U0001D6EA"
                let kappa = "\U0001D6EB"
                let lamda = "\U0001D6EC"
                let mu = "\U0001D6ED"
                let nu = "\U0001D6EE"
                let xi = "\U0001D6EF"
                let omicron = "\U0001D6F0"
                let pi = "\U0001D6F1"
                let rho = "\U0001D6F2"
                let thetaSymbol = "\U0001D6F3"
                let sigma = "\U0001D6F4"
                let tau = "\U0001D6F5"
                let upsilon = "\U0001D6F6"
                let phi = "\U0001D6F7"
                let chi = "\U0001D6F8"
                let psi = "\U0001D6F9"
                let omega = "\U0001D6FA"
                let nabla = "\U0001D6FB"                
            module BoldItalic =
                let alpha = "\U0001D71C"
                let beta = "\U0001D71D"
                let gamma = "\U0001D71E"
                let delta = "\U0001D71F"
                let epsilon = "\U0001D720"
                let zeta = "\U0001D721"
                let eta = "\U0001D722"
                let theta = "\U0001D723"
                let iota = "\U0001D724"
                let kappa = "\U0001D725"
                let lamda = "\U0001D726"
                let mu = "\U0001D727"
                let nu = "\U0001D728"
                let xi = "\U0001D729"
                let omicron = "\U0001D72A"
                let pi = "\U0001D72B"
                let rho = "\U0001D72C"
                let thetaSymbol = "\U0001D72D"
                let sigma = "\U0001D72E"
                let tau = "\U0001D72F"
                let upsilon = "\U0001D730"
                let phi = "\U0001D731"
                let chi = "\U0001D732"
                let psi = "\U0001D733"
                let omega = "\U0001D734"
                let nabla = "\U0001D735"
            module BoldSansSerif =
                let alpha = "\U0001D756"
                let beta = "\U0001D757"
                let gamma = "\U0001D758"
                let delta = "\U0001D759"
                let epsilon = "\U0001D75A"
                let zeta = "\U0001D75B"
                let eta = "\U0001D75C"
                let theta = "\U0001D75D"
                let iota = "\U0001D75E"
                let kappa = "\U0001D75F"
                let lamda = "\U0001D760"
                let mu = "\U0001D761"
                let nu = "\U0001D762"
                let xi = "\U0001D763"
                let omicron = "\U0001D764"
                let pi = "\U0001D765"
                let rho = "\U0001D766"
                let thetaSymbol = "\U0001D767"
                let sigma = "\U0001D768"
                let tau = "\U0001D769"
                let upsilon = "\U0001D76A"
                let phi = "\U0001D76B"
                let chi = "\U0001D76C"
                let psi = "\U0001D76D"
                let omega = "\U0001D76E"
                let nabla = "\U0001D76F"
            module BoldItalicSansSerif =
                let alpha = "\U0001D790"
                let beta = "\U0001D791"
                let gamma = "\U0001D792"
                let delta = "\U0001D793"
                let epsilon = "\U0001D794"
                let zeta = "\U0001D795"
                let eta = "\U0001D796"
                let theta = "\U0001D797"
                let iota = "\U0001D798"
                let kappa = "\U0001D799"
                let lamda = "\U0001D79A"
                let mu = "\U0001D79B"
                let nu = "\U0001D79C"
                let xi = "\U0001D79D"
                let omicron = "\U0001D79E"
                let pi = "\U0001D79F"
                let rho = "\U0001D7A0"
                let thetaSymbol = "\U0001D7A1"
                let sigma = "\U0001D7A2"
                let tau = "\U0001D7A3"
                let upsilon = "\U0001D7A4"
                let phi = "\U0001D7A5"
                let chi = "\U0001D7A6"
                let psi = "\U0001D7A7"
                let omega = "\U0001D7A8"
                let nabla = "\U0001D7A9"
        module Small =
            module Normal =
                let alpha = "\u03B1"
                let beta = "\u03B2"
                let gamma = "\u03B3"
                let delta = "\u03B4"
                let epsilon = "\u03B5"
                let zeta = "\u03B6"
                let eta = "\u03B7"
                let theta = "\u03B8"
                let iota = "\u03B9"
                let kappa = "\u03BA"
                let lamda = "\u03BB"
                let mu = "\u03BC"
                let nu = "\u03BD"
                let xi = "\u03BE"
                let omicron = "\u03BF"
                let pi = "\u03C0"
                let rho = "\u03C1"
                let finalSigma = "\u03C2"
                let sigma = "\u03C3"
                let tau = "\u03C4"
                let upsilon = "\u03C5"
                let phi = "\u03C6"
                let chi = "\u03C7"
                let psi = "\u03C8"
                let omega = "\u03C9"
                let PartialDifferential = "\u2202"
                let LunateEpsilonSymbol = "\u03F5"
                let ThetaSymbol = "\u03D1"
                let KappaSymbol = "\u03F0"
                let phiSymbol = "\u03D5"
                let RhoSymbol = "\u03F1"
                let piSymbol = "\u03D6"                
                let digamma = "\u03DD"
            module Bold =
                let alpha = "\U0001D6C2"
                let beta = "\U0001D6C3"
                let gamma = "\U0001D6C4"
                let delta = "\U0001D6C5"
                let epsilon = "\U0001D6C6"
                let zeta = "\U0001D6C7"
                let eta = "\U0001D6C8"
                let theta = "\U0001D6C9"
                let iota = "\U0001D6CA"
                let kappa = "\U0001D6CB"
                let lamda = "\U0001D6CC"
                let mu = "\U0001D6CD"
                let nu = "\U0001D6CE"
                let xi = "\U0001D6CF"
                let omicron = "\U0001D6D0"
                let pi = "\U0001D6D1"
                let rho = "\U0001D6D2"
                let finalSigma = "\U0001D6D3"
                let sigma = "\U0001D6D4"
                let tau = "\U0001D6D5"
                let upsilon = "\U0001D6D6"
                let phi = "\U0001D6D7"
                let chi = "\U0001D6D8"
                let psi = "\U0001D6D9"
                let omega = "\U0001D6DA"
                let PartialDifferential = "\U0001D6DB"
                let LunateEpsilonSymbol = "\U0001D6DC"
                let ThetaSymbol = "\U0001D6DD"
                let KappaSymbol = "\U0001D6DE"
                let phiSymbol = "\U0001D6DF"
                let RhoSymbol = "\U0001D6E0"
                let piSymbol = "\U0001D6E1"
                let digamma = "\U0001D7CB"
            module Italic =
                let alpha = "\U0001D6FC"
                let beta = "\U0001D6FD"
                let gamma = "\U0001D6FE"
                let delta = "\U0001D6FF"
                let epsilon = "\U0001D700"
                let zeta = "\U0001D701"
                let eta = "\U0001D702"
                let theta = "\U0001D703"
                let iota = "\U0001D704"
                let kappa = "\U0001D705"
                let lamda = "\U0001D706"
                let mu = "\U0001D707"
                let nu = "\U0001D708"
                let xi = "\U0001D709"
                let omicron = "\U0001D70A"
                let pi = "\U0001D70B"
                let rho = "\U0001D70C"
                let finalSigma = "\U0001D70D"
                let sigma = "\U0001D70E"
                let tau = "\U0001D70F"
                let upsilon = "\U0001D710"
                let phi = "\U0001D711"
                let chi = "\U0001D712"
                let psi = "\U0001D713"
                let omega = "\U0001D714"
                let PartialDifferential = "\U0001D715"
                let LunateEpsilonSymbol = "\U0001D716"
                let ThetaSymbol = "\U0001D717"
                let KappaSymbol = "\U0001D718"
                let phiSymbol = "\U0001D719"
                let RhoSymbol = "\U0001D71A"
                let piSymbol = "\U0001D71B"                
            module BoldItalic =
                let alpha = "\U0001D736"
                let beta = "\U0001D737"
                let gamma = "\U0001D738"
                let delta = "\U0001D739"
                let epsilon = "\U0001D73A"
                let zeta = "\U0001D73B"
                let eta = "\U0001D73C"
                let theta = "\U0001D73D"
                let iota = "\U0001D73E"
                let kappa = "\U0001D73F"
                let lamda = "\U0001D740"
                let mu = "\U0001D741"
                let nu = "\U0001D742"
                let xi = "\U0001D743"
                let omicron = "\U0001D744"
                let pi = "\U0001D745"
                let rho = "\U0001D746"
                let finalSigma = "\U0001D747"
                let sigma = "\U0001D748"
                let tau = "\U0001D749"
                let upsilon = "\U0001D74A"
                let phi = "\U0001D74B"
                let chi = "\U0001D74C"
                let psi = "\U0001D74D"
                let omega = "\U0001D74E"
                let PartialDifferential = "\U0001D74F"
                let LunateEpsilonSymbol = "\U0001D750"
                let ThetaSymbol = "\U0001D751"
                let KappaSymbol = "\U0001D752"
                let phiSymbol = "\U0001D753"
                let RhoSymbol = "\U0001D754"
                let piSymbol = "\U0001D755"
            module BoldSansSerif =
                let alpha = "\U0001D770"
                let beta = "\U0001D771"
                let gamma = "\U0001D772"
                let delta = "\U0001D773"
                let epsilon = "\U0001D774"
                let zeta = "\U0001D775"
                let eta = "\U0001D776"
                let theta = "\U0001D777"
                let iota = "\U0001D778"
                let kappa = "\U0001D779"
                let lamda = "\U0001D77A"
                let mu = "\U0001D77B"
                let nu = "\U0001D77C"
                let xi = "\U0001D77D"
                let omicron = "\U0001D77E"
                let pi = "\U0001D77F"
                let rho = "\U0001D780"
                let finalSigma = "\U0001D781"
                let sigma = "\U0001D782"
                let tau = "\U0001D783"
                let upsilon = "\U0001D784"
                let phi = "\U0001D785"
                let chi = "\U0001D786"
                let psi = "\U0001D787"
                let omega = "\U0001D788"
                let PartialDifferential = "\U0001D789"
                let LunateEpsilonSymbol = "\U0001D78A"
                let ThetaSymbol = "\U0001D78B"
                let KappaSymbol = "\U0001D78C"
                let phiSymbol = "\U0001D78D"
                let RhoSymbol = "\U0001D78E"
                let piSymbol = "\U0001D78F"
            module BoldItalicSansSerif =
                let alpha = "\U0001D7AA"
                let beta = "\U0001D7AB"
                let gamma = "\U0001D7AC"
                let delta = "\U0001D7AD"
                let epsilon = "\U0001D7AE"
                let zeta = "\U0001D7AF"
                let eta = "\U0001D7B0"
                let theta = "\U0001D7B1"
                let iota = "\U0001D7B2"
                let kappa = "\U0001D7B3"
                let lamda = "\U0001D7B4"
                let mu = "\U0001D7B5"
                let nu = "\U0001D7B6"
                let xi = "\U0001D7B7"
                let omicron = "\U0001D7B8"
                let pi = "\U0001D7B9"
                let rho = "\U0001D7BA"
                let finalSigma = "\U0001D7BB"
                let sigma = "\U0001D7BC"
                let tau = "\U0001D7BD"
                let upsilon = "\U0001D7BE"
                let phi = "\U0001D7BF"
                let chi = "\U0001D7C0"
                let psi = "\U0001D7C1"
                let omega = "\U0001D7C2"
                let PartialDifferential = "\U0001D7C3"
                let LunateEpsilonSymbol = "\U0001D7C4"
                let ThetaSymbol = "\U0001D7C5"
                let KappaSymbol = "\U0001D7C6"
                let phiSymbol = "\U0001D7C7"
                let RhoSymbol = "\U0001D7C8"
                let piSymbol = "\U0001D7C9"
    
    module Digit =
        module Normal =
            let zero = "\u0030"
            let one = "\u0031"
            let two = "\u0032"
            let three = "\u0033"
            let four = "\u0034"
            let five = "\u0035"
            let six = "\u0036"
            let seven = "\u0037"
            let eight = "\u0038"
            let nine = "\u0039"        
        module Bold =
            let zero = "\U0001D7CE"
            let one = "\U0001D7CF"
            let two = "\U0001D7D0"
            let three = "\U0001D7D1"
            let four = "\U0001D7D2"
            let five = "\U0001D7D3"
            let six = "\U0001D7D4"
            let seven = "\U0001D7D5"
            let eight = "\U0001D7D6"
            let nine = "\U0001D7D7"
        module DoubleStruck =
            let zero = "\U0001D7D8"
            let one = "\U0001D7D9"
            let two = "\U0001D7DA"
            let three = "\U0001D7DB"
            let four = "\U0001D7DC"
            let five = "\U0001D7DD"
            let six = "\U0001D7DE"
            let seven = "\U0001D7DF"
            let eight = "\U0001D7E0"
            let nine = "\U0001D7E1"
        module SansSerif =
            let zero = "\U0001D7E2"
            let one = "\U0001D7E3"
            let two = "\U0001D7E4"
            let three = "\U0001D7E5"
            let four = "\U0001D7E6"
            let five = "\U0001D7E7"
            let six = "\U0001D7E8"
            let seven = "\U0001D7E9"
            let eight = "\U0001D7EA"
            let nine = "\U0001D7EB"
        module SansSerifBold =
            let zero = "\U0001D7EC"
            let one = "\U0001D7ED"
            let two = "\U0001D7EE"
            let three = "\U0001D7EF"
            let four = "\U0001D7F0"
            let five = "\U0001D7F1"
            let six = "\U0001D7F2"
            let seven = "\U0001D7F3"
            let eight = "\U0001D7F4"
            let nine = "\U0001D7F5"
        module MonoSpaced =
            let zero = "\U0001D7F6"
            let one = "\U0001D7F7"
            let two = "\U0001D7F8"
            let three = "\U0001D7F9"
            let four = "\U0001D7FA"
            let five = "\U0001D7FB"
            let six = "\U0001D7FC"
            let seven = "\U0001D7FD"
            let eight = "\U0001D7FE"
            let nine = "\U0001D7FF"

module MathematicalAlphanumericSymbolMap = 
    open MathML
    open MathematicalAlphanumericSymbols

    let stringToVariant string (variant:_MathVariant) = 
        match string, variant with 
        |"0", Bold -> Digit.Bold.zero
        |"0", DoubleStruck -> Digit.DoubleStruck.zero
        |"0", MonoSpace -> Digit.MonoSpaced.zero
        |"0", Normal -> Digit.Normal.zero
        |"0", SansSerif -> Digit.SansSerif.zero
        |"0", SansSerifBold -> Digit.SansSerifBold.zero
        |"1", Bold -> Digit.Bold.one
        |"1", DoubleStruck -> Digit.DoubleStruck.one
        |"1", MonoSpace -> Digit.MonoSpaced.one
        |"1", Normal -> Digit.Normal.one
        |"1", SansSerif -> Digit.SansSerif.one
        |"1", SansSerifBold -> Digit.SansSerifBold.one
        |"2", Bold -> Digit.Bold.two
        |"2", DoubleStruck -> Digit.DoubleStruck.two
        |"2", MonoSpace -> Digit.MonoSpaced.two
        |"2", Normal -> Digit.Normal.two
        |"2", SansSerif -> Digit.SansSerif.two
        |"2", SansSerifBold -> Digit.SansSerifBold.two
        |"3", Bold -> Digit.Bold.three
        |"3", DoubleStruck -> Digit.DoubleStruck.three
        |"3", MonoSpace -> Digit.MonoSpaced.three
        |"3", Normal -> Digit.Normal.three
        |"3", SansSerif -> Digit.SansSerif.three
        |"3", SansSerifBold -> Digit.SansSerifBold.three
        |"4", Bold -> Digit.Bold.four
        |"4", DoubleStruck -> Digit.DoubleStruck.four
        |"4", MonoSpace -> Digit.MonoSpaced.four
        |"4", Normal -> Digit.Normal.four
        |"4", SansSerif -> Digit.SansSerif.four
        |"4", SansSerifBold -> Digit.SansSerifBold.four
        |"5", Bold -> Digit.Bold.five
        |"5", DoubleStruck -> Digit.DoubleStruck.five
        |"5", MonoSpace -> Digit.MonoSpaced.five
        |"5", Normal -> Digit.Normal.five
        |"5", SansSerif -> Digit.SansSerif.five
        |"5", SansSerifBold -> Digit.SansSerifBold.five
        |"6", Bold -> Digit.Bold.six
        |"6", DoubleStruck -> Digit.DoubleStruck.six
        |"6", MonoSpace -> Digit.MonoSpaced.six
        |"6", Normal -> Digit.Normal.six
        |"6", SansSerif -> Digit.SansSerif.six
        |"6", SansSerifBold -> Digit.SansSerifBold.six
        |"7", Bold -> Digit.Bold.seven
        |"7", DoubleStruck -> Digit.DoubleStruck.seven
        |"7", MonoSpace -> Digit.MonoSpaced.seven
        |"7", Normal -> Digit.Normal.seven
        |"7", SansSerif -> Digit.SansSerif.seven
        |"7", SansSerifBold -> Digit.SansSerifBold.seven
        |"8", Bold -> Digit.Bold.eight
        |"8", DoubleStruck -> Digit.DoubleStruck.eight
        |"8", MonoSpace -> Digit.MonoSpaced.eight
        |"8", Normal -> Digit.Normal.eight
        |"8", SansSerif -> Digit.SansSerif.eight
        |"8", SansSerifBold -> Digit.SansSerifBold.eight
        |"9", Bold -> Digit.Bold.nine
        |"9", DoubleStruck -> Digit.DoubleStruck.nine
        |"9", MonoSpace -> Digit.MonoSpaced.nine        
        |"9", Normal -> Digit.Normal.nine
        |"9", SansSerif -> Digit.SansSerif.nine
        |"9", SansSerifBold -> Digit.SansSerifBold.nine
        |"a", Bold -> LatinSerif.Bold.a
        |"A", Bold -> LatinSerif.Bold.A
        |"a", BoldItalic -> LatinSerif.BoldItalic.a
        |"A", BoldItalic -> LatinSerif.BoldItalic.A
        |"a", DoubleStruck -> DoubleStruck.a
        |"A", DoubleStruck -> DoubleStruck.A
        |"a", Fraktur -> Fraktur.Normal.a
        |"A", Fraktur -> Fraktur.Normal.A
        |"a", FrakturBold -> Fraktur.Bold.a
        |"A", FrakturBold -> Fraktur.Bold.A
        |"a", Italic -> LatinSerif.Italic.a
        |"A", Italic -> LatinSerif.Italic.A
        |"a", LatinScript -> LatinScript.Normal.a
        |"A", LatinScript -> LatinScript.Normal.A
        |"a", LatinScriptBold -> LatinScript.Bold.a
        |"A", LatinScriptBold -> LatinScript.Bold.A
        |"a", MonoSpace -> MonoSpaced.a
        |"A", MonoSpace -> MonoSpaced.A
        |"a", Normal -> LatinSerif.Normal.a
        |"A", Normal -> LatinSerif.Normal.A
        |"a", SansSerif -> LatinSansSerif.Normal.a
        |"A", SansSerif -> LatinSansSerif.Normal.A
        |"a", SansSerifBold -> LatinSansSerif.Bold.a
        |"A", SansSerifBold -> LatinSansSerif.Bold.A
        |"a", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.a
        |"A", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.A
        |"a", SansSerifItalic -> LatinSansSerif.Italic.a
        |"A", SansSerifItalic -> LatinSansSerif.Italic.A
        |"Alpha", Bold  -> Greek.Capital.Bold.alpha
        |"Alpha", BoldItalic  -> Greek.Capital.BoldItalic.alpha
        |"Alpha", Italic  -> Greek.Capital.Italic.alpha
        |"Alpha", Normal  -> Greek.Capital.Normal.alpha
        |"alpha", Normal -> Greek.Small.Normal.alpha
        |"Alpha", SansSerifBold  -> Greek.Capital.BoldSansSerif.alpha
        |"Alpha", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.alpha
        |"alpha", Bold  -> Greek.Small.Bold.alpha
        |"alpha", BoldItalic  -> Greek.Small.BoldItalic.alpha
        |"alpha", Italic  -> Greek.Small.Italic.alpha
        |"alpha", SansSerifBold  -> Greek.Small.BoldSansSerif.alpha
        |"alpha", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.alpha
        |"b", Bold -> LatinSerif.Bold.b
        |"B", Bold -> LatinSerif.Bold.B
        |"b", BoldItalic -> LatinSerif.BoldItalic.b
        |"B", BoldItalic -> LatinSerif.BoldItalic.B
        |"b", DoubleStruck -> DoubleStruck.b
        |"B", DoubleStruck -> DoubleStruck.B
        |"b", Fraktur -> Fraktur.Normal.b
        |"B", Fraktur -> Fraktur.Normal.B
        |"b", FrakturBold -> Fraktur.Bold.b
        |"B", FrakturBold -> Fraktur.Bold.B
        |"b", Italic -> LatinSerif.Italic.b
        |"B", Italic -> LatinSerif.Italic.B
        |"b", LatinScript -> LatinScript.Normal.b
        |"B", LatinScript -> LatinScript.Normal.B
        |"b", LatinScriptBold -> LatinScript.Bold.b
        |"B", LatinScriptBold -> LatinScript.Bold.B
        |"b", MonoSpace -> MonoSpaced.b
        |"B", MonoSpace -> MonoSpaced.B
        |"b", Normal -> LatinSerif.Normal.b
        |"B", Normal -> LatinSerif.Normal.B
        |"b", SansSerif -> LatinSansSerif.Normal.b
        |"B", SansSerif -> LatinSansSerif.Normal.B
        |"b", SansSerifBold -> LatinSansSerif.Bold.b
        |"B", SansSerifBold -> LatinSansSerif.Bold.B
        |"b", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.b
        |"B", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.B
        |"b", SansSerifItalic -> LatinSansSerif.Italic.b
        |"B", SansSerifItalic -> LatinSansSerif.Italic.B
        |"Beta", Bold  -> Greek.Capital.Bold.beta
        |"Beta", BoldItalic  -> Greek.Capital.BoldItalic.beta
        |"Beta", Italic  -> Greek.Capital.Italic.beta
        |"Beta", Normal  -> Greek.Capital.Normal.beta
        |"beta", Normal -> Greek.Small.Normal.beta
        |"Beta", SansSerifBold  -> Greek.Capital.BoldSansSerif.beta
        |"Beta", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.beta
        |"beta", Bold  -> Greek.Small.Bold.beta
        |"beta", BoldItalic  -> Greek.Small.BoldItalic.beta
        |"beta", Italic  -> Greek.Small.Italic.beta
        |"beta", SansSerifBold  -> Greek.Small.BoldSansSerif.beta
        |"beta", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.beta
        |"c", Bold -> LatinSerif.Bold.c
        |"C", Bold -> LatinSerif.Bold.C
        |"c", BoldItalic -> LatinSerif.BoldItalic.c
        |"C", BoldItalic -> LatinSerif.BoldItalic.C
        |"c", DoubleStruck -> DoubleStruck.c
        |"C", DoubleStruck -> DoubleStruck.C
        |"c", Fraktur -> Fraktur.Normal.c
        |"C", Fraktur -> Fraktur.Normal.C
        |"c", FrakturBold -> Fraktur.Bold.c
        |"C", FrakturBold -> Fraktur.Bold.C
        |"c", Italic -> LatinSerif.Italic.c
        |"C", Italic -> LatinSerif.Italic.C
        |"c", LatinScript -> LatinScript.Normal.c
        |"C", LatinScript -> LatinScript.Normal.C
        |"c", LatinScriptBold -> LatinScript.Bold.c
        |"C", LatinScriptBold -> LatinScript.Bold.C
        |"c", MonoSpace -> MonoSpaced.c
        |"C", MonoSpace -> MonoSpaced.C
        |"c", Normal -> LatinSerif.Normal.c
        |"C", Normal -> LatinSerif.Normal.C
        |"c", SansSerif -> LatinSansSerif.Normal.c
        |"C", SansSerif -> LatinSansSerif.Normal.C
        |"c", SansSerifBold -> LatinSansSerif.Bold.c
        |"C", SansSerifBold -> LatinSansSerif.Bold.C
        |"c", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.c
        |"C", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.C
        |"c", SansSerifItalic -> LatinSansSerif.Italic.c
        |"C", SansSerifItalic -> LatinSansSerif.Italic.C
        |"Chi", Bold  -> Greek.Capital.Bold.chi
        |"Chi", BoldItalic  -> Greek.Capital.BoldItalic.chi
        |"Chi", Italic  -> Greek.Capital.Italic.chi
        |"Chi", Normal  -> Greek.Capital.Normal.chi
        |"chi", Normal -> Greek.Small.Normal.chi
        |"Chi", SansSerifBold  -> Greek.Capital.BoldSansSerif.chi
        |"Chi", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.chi
        |"chi", Bold  -> Greek.Small.Bold.chi
        |"chi", BoldItalic  -> Greek.Small.BoldItalic.chi
        |"chi", Italic  -> Greek.Small.Italic.chi
        |"chi", SansSerifBold  -> Greek.Small.BoldSansSerif.chi
        |"chi", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.chi
        |"d", Bold -> LatinSerif.Bold.d
        |"D", Bold -> LatinSerif.Bold.D
        |"d", BoldItalic -> LatinSerif.BoldItalic.d
        |"D", BoldItalic -> LatinSerif.BoldItalic.D
        |"d", DoubleStruck -> DoubleStruck.d
        |"D", DoubleStruck -> DoubleStruck.D
        |"d", Fraktur -> Fraktur.Normal.d
        |"D", Fraktur -> Fraktur.Normal.D
        |"d", FrakturBold -> Fraktur.Bold.d
        |"D", FrakturBold -> Fraktur.Bold.D
        |"d", Italic -> LatinSerif.Italic.d
        |"D", Italic -> LatinSerif.Italic.D
        |"d", LatinScript -> LatinScript.Normal.d
        |"D", LatinScript -> LatinScript.Normal.D
        |"d", LatinScriptBold -> LatinScript.Bold.d
        |"D", LatinScriptBold -> LatinScript.Bold.D
        |"d", MonoSpace -> MonoSpaced.d
        |"D", MonoSpace -> MonoSpaced.D
        |"d", Normal -> LatinSerif.Normal.d
        |"D", Normal -> LatinSerif.Normal.D
        |"d", SansSerif -> LatinSansSerif.Normal.d
        |"D", SansSerif -> LatinSansSerif.Normal.D
        |"d", SansSerifBold -> LatinSansSerif.Bold.d
        |"D", SansSerifBold -> LatinSansSerif.Bold.D
        |"d", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.d
        |"D", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.D
        |"d", SansSerifItalic -> LatinSansSerif.Italic.d
        |"D", SansSerifItalic -> LatinSansSerif.Italic.D
        |"Delta", Bold  -> Greek.Capital.Bold.delta
        |"Delta", BoldItalic  -> Greek.Capital.BoldItalic.delta
        |"Delta", Italic  -> Greek.Capital.Italic.delta
        |"Delta", Normal  -> Greek.Capital.Normal.delta
        |"delta", Normal -> Greek.Small.Normal.delta
        |"Delta", SansSerifBold  -> Greek.Capital.BoldSansSerif.delta
        |"Delta", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.delta
        |"delta", Bold  -> Greek.Small.Bold.delta
        |"delta", BoldItalic  -> Greek.Small.BoldItalic.delta
        |"delta", Italic  -> Greek.Small.Italic.delta
        |"delta", SansSerifBold  -> Greek.Small.BoldSansSerif.delta
        |"delta", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.delta
        |"Digamma", Bold  -> Greek.Capital.Bold.digamma
        |"Digamma", Normal  -> Greek.Capital.Normal.digamma
        |"digamma", Normal -> Greek.Small.Normal.digamma
        |"digamma", Bold  -> Greek.Small.Bold.digamma
        |"e", Bold -> LatinSerif.Bold.e
        |"E", Bold -> LatinSerif.Bold.E
        |"e", BoldItalic -> LatinSerif.BoldItalic.e
        |"E", BoldItalic -> LatinSerif.BoldItalic.E
        |"e", DoubleStruck -> DoubleStruck.e
        |"E", DoubleStruck -> DoubleStruck.E
        |"e", Fraktur -> Fraktur.Normal.e
        |"E", Fraktur -> Fraktur.Normal.E
        |"e", FrakturBold -> Fraktur.Bold.e
        |"E", FrakturBold -> Fraktur.Bold.E
        |"e", Italic -> LatinSerif.Italic.e
        |"E", Italic -> LatinSerif.Italic.E
        |"e", LatinScript -> LatinScript.Normal.e
        |"E", LatinScript -> LatinScript.Normal.E
        |"e", LatinScriptBold -> LatinScript.Bold.e
        |"E", LatinScriptBold -> LatinScript.Bold.E
        |"e", MonoSpace -> MonoSpaced.e
        |"E", MonoSpace -> MonoSpaced.E
        |"e", Normal -> LatinSerif.Normal.e
        |"E", Normal -> LatinSerif.Normal.E
        |"e", SansSerif -> LatinSansSerif.Normal.e
        |"E", SansSerif -> LatinSansSerif.Normal.E
        |"e", SansSerifBold -> LatinSansSerif.Bold.e
        |"E", SansSerifBold -> LatinSansSerif.Bold.E
        |"e", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.e
        |"E", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.E
        |"e", SansSerifItalic -> LatinSansSerif.Italic.e
        |"E", SansSerifItalic -> LatinSansSerif.Italic.E
        |"Epsilon", Bold  -> Greek.Capital.Bold.epsilon
        |"Epsilon", BoldItalic  -> Greek.Capital.BoldItalic.epsilon
        |"Epsilon", Italic  -> Greek.Capital.Italic.epsilon
        |"Epsilon", Normal  -> Greek.Capital.Normal.epsilon
        |"epsilon", Normal -> Greek.Small.Normal.epsilon
        |"Epsilon", SansSerifBold  -> Greek.Capital.BoldSansSerif.epsilon
        |"Epsilon", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.epsilon
        |"epsilon", Bold  -> Greek.Small.Bold.epsilon
        |"epsilon", BoldItalic  -> Greek.Small.BoldItalic.epsilon
        |"epsilon", Italic  -> Greek.Small.Italic.epsilon
        |"epsilon", SansSerifBold  -> Greek.Small.BoldSansSerif.epsilon
        |"epsilon", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.epsilon
        |"Eta", Bold  -> Greek.Capital.Bold.eta
        |"Eta", BoldItalic  -> Greek.Capital.BoldItalic.eta
        |"Eta", Italic  -> Greek.Capital.Italic.eta
        |"Eta", Normal  -> Greek.Capital.Normal.eta
        |"eta", Normal -> Greek.Small.Normal.eta
        |"Eta", SansSerifBold  -> Greek.Capital.BoldSansSerif.eta
        |"Eta", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.eta
        |"eta", Bold  -> Greek.Small.Bold.eta
        |"eta", BoldItalic  -> Greek.Small.BoldItalic.eta
        |"eta", Italic  -> Greek.Small.Italic.eta
        |"eta", SansSerifBold  -> Greek.Small.BoldSansSerif.eta
        |"eta", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.eta
        |"f", Bold -> LatinSerif.Bold.f
        |"F", Bold -> LatinSerif.Bold.F
        |"f", BoldItalic -> LatinSerif.BoldItalic.f
        |"F", BoldItalic -> LatinSerif.BoldItalic.F
        |"f", DoubleStruck -> DoubleStruck.f
        |"F", DoubleStruck -> DoubleStruck.F
        |"f", Fraktur -> Fraktur.Normal.f
        |"F", Fraktur -> Fraktur.Normal.F
        |"f", FrakturBold -> Fraktur.Bold.f
        |"F", FrakturBold -> Fraktur.Bold.F
        |"f", Italic -> LatinSerif.Italic.f
        |"F", Italic -> LatinSerif.Italic.F
        |"f", LatinScript -> LatinScript.Normal.f
        |"F", LatinScript -> LatinScript.Normal.F
        |"f", LatinScriptBold -> LatinScript.Bold.f
        |"F", LatinScriptBold -> LatinScript.Bold.F
        |"f", MonoSpace -> MonoSpaced.f
        |"F", MonoSpace -> MonoSpaced.F
        |"f", Normal -> LatinSerif.Normal.f
        |"F", Normal -> LatinSerif.Normal.F
        |"f", SansSerif -> LatinSansSerif.Normal.f
        |"F", SansSerif -> LatinSansSerif.Normal.F
        |"f", SansSerifBold -> LatinSansSerif.Bold.f
        |"F", SansSerifBold -> LatinSansSerif.Bold.F
        |"f", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.f
        |"F", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.F
        |"f", SansSerifItalic -> LatinSansSerif.Italic.f
        |"F", SansSerifItalic -> LatinSansSerif.Italic.F
        |"finalSigma", Normal -> Greek.Small.Normal.finalSigma
        |"finalSigma", Bold  -> Greek.Small.Bold.finalSigma
        |"finalSigma", BoldItalic  -> Greek.Small.BoldItalic.finalSigma
        |"finalSigma", Italic  -> Greek.Small.Italic.finalSigma
        |"finalSigma", SansSerifBold  -> Greek.Small.BoldSansSerif.finalSigma
        |"finalSigma", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.finalSigma
        |"g", Bold -> LatinSerif.Bold.g
        |"G", Bold -> LatinSerif.Bold.G
        |"g", BoldItalic -> LatinSerif.BoldItalic.g
        |"G", BoldItalic -> LatinSerif.BoldItalic.G
        |"g", DoubleStruck -> DoubleStruck.g
        |"G", DoubleStruck -> DoubleStruck.G
        |"g", Fraktur -> Fraktur.Normal.g
        |"G", Fraktur -> Fraktur.Normal.G
        |"g", FrakturBold -> Fraktur.Bold.g
        |"G", FrakturBold -> Fraktur.Bold.G
        |"g", Italic -> LatinSerif.Italic.g
        |"G", Italic -> LatinSerif.Italic.G
        |"g", LatinScript -> LatinScript.Normal.g
        |"G", LatinScript -> LatinScript.Normal.G
        |"g", LatinScriptBold -> LatinScript.Bold.g
        |"G", LatinScriptBold -> LatinScript.Bold.G
        |"g", MonoSpace -> MonoSpaced.g
        |"G", MonoSpace -> MonoSpaced.G
        |"g", Normal -> LatinSerif.Normal.g
        |"G", Normal -> LatinSerif.Normal.G
        |"g", SansSerif -> LatinSansSerif.Normal.g
        |"G", SansSerif -> LatinSansSerif.Normal.G
        |"g", SansSerifBold -> LatinSansSerif.Bold.g
        |"G", SansSerifBold -> LatinSansSerif.Bold.G
        |"g", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.g
        |"G", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.G
        |"g", SansSerifItalic -> LatinSansSerif.Italic.g
        |"G", SansSerifItalic -> LatinSansSerif.Italic.G
        |"Gamma", Bold  -> Greek.Capital.Bold.gamma
        |"Gamma", BoldItalic  -> Greek.Capital.BoldItalic.gamma
        |"Gamma", Italic  -> Greek.Capital.Italic.gamma
        |"Gamma", Normal  -> Greek.Capital.Normal.gamma
        |"gamma", Normal -> Greek.Small.Normal.gamma
        |"Gamma", SansSerifBold  -> Greek.Capital.BoldSansSerif.gamma
        |"Gamma", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.gamma
        |"gamma", Bold  -> Greek.Small.Bold.gamma
        |"gamma", BoldItalic  -> Greek.Small.BoldItalic.gamma
        |"gamma", Italic  -> Greek.Small.Italic.gamma
        |"gamma", SansSerifBold  -> Greek.Small.BoldSansSerif.gamma
        |"gamma", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.gamma
        |"h", Bold -> LatinSerif.Bold.h
        |"H", Bold -> LatinSerif.Bold.H
        |"h", BoldItalic -> LatinSerif.BoldItalic.h
        |"H", BoldItalic -> LatinSerif.BoldItalic.H
        |"h", DoubleStruck -> DoubleStruck.h
        |"H", DoubleStruck -> DoubleStruck.H
        |"h", Fraktur -> Fraktur.Normal.h
        |"H", Fraktur -> Fraktur.Normal.H
        |"h", FrakturBold -> Fraktur.Bold.h
        |"H", FrakturBold -> Fraktur.Bold.H
        |"h", Italic -> LatinSerif.Italic.h
        |"H", Italic -> LatinSerif.Italic.H
        |"h", LatinScript -> LatinScript.Normal.h
        |"H", LatinScript -> LatinScript.Normal.H
        |"h", LatinScriptBold -> LatinScript.Bold.h
        |"H", LatinScriptBold -> LatinScript.Bold.H
        |"h", MonoSpace -> MonoSpaced.h
        |"H", MonoSpace -> MonoSpaced.H
        |"h", Normal -> LatinSerif.Normal.h
        |"H", Normal -> LatinSerif.Normal.H
        |"h", SansSerif -> LatinSansSerif.Normal.h
        |"H", SansSerif -> LatinSansSerif.Normal.H
        |"h", SansSerifBold -> LatinSansSerif.Bold.h
        |"H", SansSerifBold -> LatinSansSerif.Bold.H
        |"h", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.h
        |"H", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.H
        |"h", SansSerifItalic -> LatinSansSerif.Italic.h
        |"H", SansSerifItalic -> LatinSansSerif.Italic.H
        |"i", Bold -> LatinSerif.Bold.i
        |"I", Bold -> LatinSerif.Bold.I
        |"i", BoldItalic -> LatinSerif.BoldItalic.i
        |"I", BoldItalic -> LatinSerif.BoldItalic.I
        |"i", DoubleStruck -> DoubleStruck.i
        |"I", DoubleStruck -> DoubleStruck.I
        |"i", Fraktur -> Fraktur.Normal.i
        |"I", Fraktur -> Fraktur.Normal.I
        |"i", FrakturBold -> Fraktur.Bold.i
        |"I", FrakturBold -> Fraktur.Bold.I
        |"i", Italic -> LatinSerif.Italic.i
        |"I", Italic -> LatinSerif.Italic.I
        |"i", LatinScript -> LatinScript.Normal.i
        |"I", LatinScript -> LatinScript.Normal.I
        |"i", LatinScriptBold -> LatinScript.Bold.i
        |"I", LatinScriptBold -> LatinScript.Bold.I
        |"i", MonoSpace -> MonoSpaced.i
        |"I", MonoSpace -> MonoSpaced.I
        |"i", Normal -> LatinSerif.Normal.i
        |"I", Normal -> LatinSerif.Normal.I
        |"i", SansSerif -> LatinSansSerif.Normal.i
        |"I", SansSerif -> LatinSansSerif.Normal.I
        |"i", SansSerifBold -> LatinSansSerif.Bold.i
        |"I", SansSerifBold -> LatinSansSerif.Bold.I
        |"i", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.i
        |"I", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.I
        |"i", SansSerifItalic -> LatinSansSerif.Italic.i
        |"I", SansSerifItalic -> LatinSansSerif.Italic.I
        |"iDotless", Italic -> LatinSerif.Italic.iDotless
        |"iDotless", Normal -> LatinSerif.Normal.iDotless
        |"Iota", Bold  -> Greek.Capital.Bold.iota
        |"Iota", BoldItalic  -> Greek.Capital.BoldItalic.iota
        |"Iota", Italic  -> Greek.Capital.Italic.iota
        |"Iota", Normal  -> Greek.Capital.Normal.iota
        |"iota", Normal -> Greek.Small.Normal.iota
        |"Iota", SansSerifBold  -> Greek.Capital.BoldSansSerif.iota
        |"Iota", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.iota
        |"iota", Bold  -> Greek.Small.Bold.iota
        |"iota", BoldItalic  -> Greek.Small.BoldItalic.iota
        |"iota", Italic  -> Greek.Small.Italic.iota
        |"iota", SansSerifBold  -> Greek.Small.BoldSansSerif.iota
        |"iota", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.iota
        |"j", Bold -> LatinSerif.Bold.j
        |"J", Bold -> LatinSerif.Bold.J
        |"j", BoldItalic -> LatinSerif.BoldItalic.j
        |"J", BoldItalic -> LatinSerif.BoldItalic.J
        |"j", DoubleStruck -> DoubleStruck.j
        |"J", DoubleStruck -> DoubleStruck.J
        |"j", Fraktur -> Fraktur.Normal.j
        |"J", Fraktur -> Fraktur.Normal.J
        |"j", FrakturBold -> Fraktur.Bold.j
        |"J", FrakturBold -> Fraktur.Bold.J
        |"j", Italic -> LatinSerif.Italic.j
        |"J", Italic -> LatinSerif.Italic.J
        |"j", LatinScript -> LatinScript.Normal.j
        |"J", LatinScript -> LatinScript.Normal.J
        |"j", LatinScriptBold -> LatinScript.Bold.j
        |"J", LatinScriptBold -> LatinScript.Bold.J
        |"j", MonoSpace -> MonoSpaced.j
        |"J", MonoSpace -> MonoSpaced.J
        |"j", Normal -> LatinSerif.Normal.j
        |"J", Normal -> LatinSerif.Normal.J
        |"j", SansSerif -> LatinSansSerif.Normal.j
        |"J", SansSerif -> LatinSansSerif.Normal.J
        |"j", SansSerifBold -> LatinSansSerif.Bold.j
        |"J", SansSerifBold -> LatinSansSerif.Bold.J
        |"j", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.j
        |"J", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.J
        |"j", SansSerifItalic -> LatinSansSerif.Italic.j
        |"J", SansSerifItalic -> LatinSansSerif.Italic.J
        |"jDotless", Italic -> LatinSerif.Italic.jDotless
        |"jDotless", Normal -> LatinSerif.Normal.jDotless
        |"k", Bold -> LatinSerif.Bold.k
        |"K", Bold -> LatinSerif.Bold.K
        |"k", BoldItalic -> LatinSerif.BoldItalic.k
        |"K", BoldItalic -> LatinSerif.BoldItalic.K
        |"k", DoubleStruck -> DoubleStruck.k
        |"K", DoubleStruck -> DoubleStruck.K
        |"k", Fraktur -> Fraktur.Normal.k
        |"K", Fraktur -> Fraktur.Normal.K
        |"k", FrakturBold -> Fraktur.Bold.k
        |"K", FrakturBold -> Fraktur.Bold.K
        |"k", Italic -> LatinSerif.Italic.k
        |"K", Italic -> LatinSerif.Italic.K
        |"k", LatinScript -> LatinScript.Normal.k
        |"K", LatinScript -> LatinScript.Normal.K
        |"k", LatinScriptBold -> LatinScript.Bold.k
        |"K", LatinScriptBold -> LatinScript.Bold.K
        |"k", MonoSpace -> MonoSpaced.k
        |"K", MonoSpace -> MonoSpaced.K
        |"k", Normal -> LatinSerif.Normal.k
        |"K", Normal -> LatinSerif.Normal.K
        |"k", SansSerif -> LatinSansSerif.Normal.k
        |"K", SansSerif -> LatinSansSerif.Normal.K
        |"k", SansSerifBold -> LatinSansSerif.Bold.k
        |"K", SansSerifBold -> LatinSansSerif.Bold.K
        |"k", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.k
        |"K", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.K
        |"k", SansSerifItalic -> LatinSansSerif.Italic.k
        |"K", SansSerifItalic -> LatinSansSerif.Italic.K
        |"Kappa", Bold  -> Greek.Capital.Bold.kappa
        |"Kappa", BoldItalic  -> Greek.Capital.BoldItalic.kappa
        |"Kappa", Italic  -> Greek.Capital.Italic.kappa
        |"Kappa", Normal  -> Greek.Capital.Normal.kappa
        |"kappa", Normal -> Greek.Small.Normal.kappa
        |"Kappa", SansSerifBold  -> Greek.Capital.BoldSansSerif.kappa
        |"Kappa", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.kappa
        |"kappa", Bold  -> Greek.Small.Bold.kappa
        |"kappa", BoldItalic  -> Greek.Small.BoldItalic.kappa
        |"kappa", Italic  -> Greek.Small.Italic.kappa
        |"kappa", SansSerifBold  -> Greek.Small.BoldSansSerif.kappa
        |"kappa", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.kappa
        |"kappaSymbol", Normal -> Greek.Small.Normal.KappaSymbol
        |"kappaSymbol", Bold  -> Greek.Small.Bold.KappaSymbol
        |"kappaSymbol", BoldItalic  -> Greek.Small.BoldItalic.KappaSymbol
        |"kappaSymbol", Italic  -> Greek.Small.Italic.KappaSymbol
        |"kappaSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.KappaSymbol
        |"kappaSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.KappaSymbol
        |"l", Bold -> LatinSerif.Bold.l
        |"L", Bold -> LatinSerif.Bold.L
        |"l", BoldItalic -> LatinSerif.BoldItalic.l
        |"L", BoldItalic -> LatinSerif.BoldItalic.L
        |"l", DoubleStruck -> DoubleStruck.l
        |"L", DoubleStruck -> DoubleStruck.L
        |"l", Fraktur -> Fraktur.Normal.l
        |"L", Fraktur -> Fraktur.Normal.L
        |"l", FrakturBold -> Fraktur.Bold.l
        |"L", FrakturBold -> Fraktur.Bold.L
        |"l", Italic -> LatinSerif.Italic.l
        |"L", Italic -> LatinSerif.Italic.L
        |"l", LatinScript -> LatinScript.Normal.l
        |"L", LatinScript -> LatinScript.Normal.L
        |"l", LatinScriptBold -> LatinScript.Bold.l
        |"L", LatinScriptBold -> LatinScript.Bold.L
        |"l", MonoSpace -> MonoSpaced.l
        |"L", MonoSpace -> MonoSpaced.L
        |"l", Normal -> LatinSerif.Normal.l
        |"L", Normal -> LatinSerif.Normal.L
        |"l", SansSerif -> LatinSansSerif.Normal.l
        |"L", SansSerif -> LatinSansSerif.Normal.L
        |"l", SansSerifBold -> LatinSansSerif.Bold.l
        |"L", SansSerifBold -> LatinSansSerif.Bold.L
        |"l", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.l
        |"L", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.L
        |"l", SansSerifItalic -> LatinSansSerif.Italic.l
        |"L", SansSerifItalic -> LatinSansSerif.Italic.L
        |"Lamda", Bold  -> Greek.Capital.Bold.lamda
        |"Lamda", BoldItalic  -> Greek.Capital.BoldItalic.lamda
        |"Lamda", Italic  -> Greek.Capital.Italic.lamda
        |"Lamda", Normal  -> Greek.Capital.Normal.lamda
        |"lamda", Normal -> Greek.Small.Normal.lamda
        |"Lamda", SansSerifBold  -> Greek.Capital.BoldSansSerif.lamda
        |"Lamda", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.lamda
        |"lamda", Bold  -> Greek.Small.Bold.lamda
        |"lamda", BoldItalic  -> Greek.Small.BoldItalic.lamda
        |"lamda", Italic  -> Greek.Small.Italic.lamda
        |"lamda", SansSerifBold  -> Greek.Small.BoldSansSerif.lamda
        |"lamda", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.lamda
        |"lunateEpsilonSymbol", Normal -> Greek.Small.Normal.LunateEpsilonSymbol
        |"lunateEpsilonSymbol", Bold  -> Greek.Small.Bold.LunateEpsilonSymbol
        |"lunateEpsilonSymbol", BoldItalic  -> Greek.Small.BoldItalic.LunateEpsilonSymbol
        |"lunateEpsilonSymbol", Italic  -> Greek.Small.Italic.LunateEpsilonSymbol
        |"lunateEpsilonSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.LunateEpsilonSymbol
        |"lunateEpsilonSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.LunateEpsilonSymbol
        |"m", Bold -> LatinSerif.Bold.m
        |"M", Bold -> LatinSerif.Bold.M
        |"m", BoldItalic -> LatinSerif.BoldItalic.m
        |"M", BoldItalic -> LatinSerif.BoldItalic.M
        |"m", DoubleStruck -> DoubleStruck.m
        |"M", DoubleStruck -> DoubleStruck.M
        |"m", Fraktur -> Fraktur.Normal.m
        |"M", Fraktur -> Fraktur.Normal.M
        |"m", FrakturBold -> Fraktur.Bold.m
        |"M", FrakturBold -> Fraktur.Bold.M
        |"m", Italic -> LatinSerif.Italic.m
        |"M", Italic -> LatinSerif.Italic.M
        |"m", LatinScript -> LatinScript.Normal.m
        |"M", LatinScript -> LatinScript.Normal.M
        |"m", LatinScriptBold -> LatinScript.Bold.m
        |"M", LatinScriptBold -> LatinScript.Bold.M
        |"m", MonoSpace -> MonoSpaced.m
        |"M", MonoSpace -> MonoSpaced.M
        |"m", Normal -> LatinSerif.Normal.m
        |"M", Normal -> LatinSerif.Normal.M
        |"m", SansSerif -> LatinSansSerif.Normal.m
        |"M", SansSerif -> LatinSansSerif.Normal.M
        |"m", SansSerifBold -> LatinSansSerif.Bold.m
        |"M", SansSerifBold -> LatinSansSerif.Bold.M
        |"m", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.m
        |"M", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.M
        |"m", SansSerifItalic -> LatinSansSerif.Italic.m
        |"M", SansSerifItalic -> LatinSansSerif.Italic.M
        |"Mu", Bold  -> Greek.Capital.Bold.mu
        |"Mu", BoldItalic  -> Greek.Capital.BoldItalic.mu
        |"Mu", Italic  -> Greek.Capital.Italic.mu
        |"Mu", Normal  -> Greek.Capital.Normal.mu
        |"mu", Normal -> Greek.Small.Normal.mu
        |"Mu", SansSerifBold  -> Greek.Capital.BoldSansSerif.mu
        |"Mu", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.mu
        |"mu", Bold  -> Greek.Small.Bold.mu
        |"mu", BoldItalic  -> Greek.Small.BoldItalic.mu
        |"mu", Italic  -> Greek.Small.Italic.mu
        |"mu", SansSerifBold  -> Greek.Small.BoldSansSerif.mu
        |"mu", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.mu
        |"n", Bold -> LatinSerif.Bold.n
        |"N", Bold -> LatinSerif.Bold.N
        |"n", BoldItalic -> LatinSerif.BoldItalic.n
        |"N", BoldItalic -> LatinSerif.BoldItalic.N
        |"n", DoubleStruck -> DoubleStruck.n
        |"N", DoubleStruck -> DoubleStruck.N
        |"n", Fraktur -> Fraktur.Normal.n
        |"N", Fraktur -> Fraktur.Normal.N
        |"n", FrakturBold -> Fraktur.Bold.n
        |"N", FrakturBold -> Fraktur.Bold.N
        |"n", Italic -> LatinSerif.Italic.n
        |"N", Italic -> LatinSerif.Italic.N
        |"n", LatinScript -> LatinScript.Normal.n
        |"N", LatinScript -> LatinScript.Normal.N
        |"n", LatinScriptBold -> LatinScript.Bold.n
        |"N", LatinScriptBold -> LatinScript.Bold.N
        |"n", MonoSpace -> MonoSpaced.n
        |"N", MonoSpace -> MonoSpaced.N
        |"n", Normal -> LatinSerif.Normal.n
        |"N", Normal -> LatinSerif.Normal.N
        |"n", SansSerif -> LatinSansSerif.Normal.n
        |"N", SansSerif -> LatinSansSerif.Normal.N
        |"n", SansSerifBold -> LatinSansSerif.Bold.n
        |"N", SansSerifBold -> LatinSansSerif.Bold.N
        |"n", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.n
        |"N", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.N
        |"n", SansSerifItalic -> LatinSansSerif.Italic.n
        |"N", SansSerifItalic -> LatinSansSerif.Italic.N
        |"Nabla", Bold  -> Greek.Capital.Bold.nabla
        |"Nabla", BoldItalic  -> Greek.Capital.BoldItalic.nabla
        |"Nabla", Italic  -> Greek.Capital.Italic.nabla
        |"Nabla", Normal  -> Greek.Capital.Normal.nabla
        |"Nabla", SansSerifBold  -> Greek.Capital.BoldSansSerif.nabla
        |"Nabla", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.nabla
        |"Nu", Bold  -> Greek.Capital.Bold.nu
        |"Nu", BoldItalic  -> Greek.Capital.BoldItalic.nu
        |"Nu", Italic  -> Greek.Capital.Italic.nu
        |"Nu", Normal  -> Greek.Capital.Normal.nu
        |"nu", Normal -> Greek.Small.Normal.nu
        |"Nu", SansSerifBold  -> Greek.Capital.BoldSansSerif.nu
        |"Nu", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.nu
        |"nu", Bold  -> Greek.Small.Bold.nu
        |"nu", BoldItalic  -> Greek.Small.BoldItalic.nu
        |"nu", Italic  -> Greek.Small.Italic.nu
        |"nu", SansSerifBold  -> Greek.Small.BoldSansSerif.nu
        |"nu", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.nu
        |"o", Bold -> LatinSerif.Bold.o
        |"O", Bold -> LatinSerif.Bold.O
        |"o", BoldItalic -> LatinSerif.BoldItalic.o
        |"O", BoldItalic -> LatinSerif.BoldItalic.O
        |"o", DoubleStruck -> DoubleStruck.o
        |"O", DoubleStruck -> DoubleStruck.O
        |"o", Fraktur -> Fraktur.Normal.o
        |"O", Fraktur -> Fraktur.Normal.O
        |"o", FrakturBold -> Fraktur.Bold.o
        |"O", FrakturBold -> Fraktur.Bold.O
        |"o", Italic -> LatinSerif.Italic.o
        |"O", Italic -> LatinSerif.Italic.O
        |"o", LatinScript -> LatinScript.Normal.o
        |"O", LatinScript -> LatinScript.Normal.O
        |"o", LatinScriptBold -> LatinScript.Bold.o
        |"O", LatinScriptBold -> LatinScript.Bold.O
        |"o", MonoSpace -> MonoSpaced.o
        |"O", MonoSpace -> MonoSpaced.O
        |"o", Normal -> LatinSerif.Normal.o
        |"O", Normal -> LatinSerif.Normal.O
        |"o", SansSerif -> LatinSansSerif.Normal.o
        |"O", SansSerif -> LatinSansSerif.Normal.O
        |"o", SansSerifBold -> LatinSansSerif.Bold.o
        |"O", SansSerifBold -> LatinSansSerif.Bold.O
        |"o", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.o
        |"O", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.O
        |"o", SansSerifItalic -> LatinSansSerif.Italic.o
        |"O", SansSerifItalic -> LatinSansSerif.Italic.O
        |"Omega", Bold  -> Greek.Capital.Bold.omega
        |"Omega", BoldItalic  -> Greek.Capital.BoldItalic.omega
        |"Omega", Italic  -> Greek.Capital.Italic.omega
        |"Omega", Normal  -> Greek.Capital.Normal.omega
        |"omega", Normal -> Greek.Small.Normal.omega
        |"Omega", SansSerifBold  -> Greek.Capital.BoldSansSerif.omega
        |"Omega", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.omega
        |"omega", Bold  -> Greek.Small.Bold.omega
        |"omega", BoldItalic  -> Greek.Small.BoldItalic.omega
        |"omega", Italic  -> Greek.Small.Italic.omega
        |"omega", SansSerifBold  -> Greek.Small.BoldSansSerif.omega
        |"omega", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.omega
        |"Omicron", Bold  -> Greek.Capital.Bold.omicron
        |"Omicron", BoldItalic  -> Greek.Capital.BoldItalic.omicron
        |"Omicron", Italic  -> Greek.Capital.Italic.omicron
        |"Omicron", Normal  -> Greek.Capital.Normal.omicron
        |"omicron", Normal -> Greek.Small.Normal.omicron
        |"Omicron", SansSerifBold  -> Greek.Capital.BoldSansSerif.omicron
        |"Omicron", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.omicron
        |"omicron", Bold  -> Greek.Small.Bold.omicron
        |"omicron", BoldItalic  -> Greek.Small.BoldItalic.omicron
        |"omicron", Italic  -> Greek.Small.Italic.omicron
        |"omicron", SansSerifBold  -> Greek.Small.BoldSansSerif.omicron
        |"omicron", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.omicron
        |"p", Bold -> LatinSerif.Bold.p
        |"P", Bold -> LatinSerif.Bold.P
        |"p", BoldItalic -> LatinSerif.BoldItalic.p
        |"P", BoldItalic -> LatinSerif.BoldItalic.P
        |"p", DoubleStruck -> DoubleStruck.p
        |"P", DoubleStruck -> DoubleStruck.P
        |"p", Fraktur -> Fraktur.Normal.p
        |"P", Fraktur -> Fraktur.Normal.P
        |"p", FrakturBold -> Fraktur.Bold.p
        |"P", FrakturBold -> Fraktur.Bold.P
        |"p", Italic -> LatinSerif.Italic.p
        |"P", Italic -> LatinSerif.Italic.P
        |"p", LatinScript -> LatinScript.Normal.p
        |"P", LatinScript -> LatinScript.Normal.P
        |"p", LatinScriptBold -> LatinScript.Bold.p
        |"P", LatinScriptBold -> LatinScript.Bold.P
        |"p", MonoSpace -> MonoSpaced.p
        |"P", MonoSpace -> MonoSpaced.P
        |"p", Normal -> LatinSerif.Normal.p
        |"P", Normal -> LatinSerif.Normal.P
        |"p", SansSerif -> LatinSansSerif.Normal.p
        |"P", SansSerif -> LatinSansSerif.Normal.P
        |"p", SansSerifBold -> LatinSansSerif.Bold.p
        |"P", SansSerifBold -> LatinSansSerif.Bold.P
        |"p", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.p
        |"P", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.P
        |"p", SansSerifItalic -> LatinSansSerif.Italic.p
        |"P", SansSerifItalic -> LatinSansSerif.Italic.P
        |"partialDifferential", Normal -> Greek.Small.Normal.PartialDifferential
        |"partialDifferential", Bold  -> Greek.Small.Bold.PartialDifferential
        |"partialDifferential", BoldItalic  -> Greek.Small.BoldItalic.PartialDifferential
        |"partialDifferential", Italic  -> Greek.Small.Italic.PartialDifferential
        |"partialDifferential", SansSerifBold  -> Greek.Small.BoldSansSerif.PartialDifferential
        |"partialDifferential", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.PartialDifferential
        |"Phi", Bold  -> Greek.Capital.Bold.phi
        |"Phi", BoldItalic  -> Greek.Capital.BoldItalic.phi
        |"Phi", Italic  -> Greek.Capital.Italic.phi
        |"Phi", Normal  -> Greek.Capital.Normal.phi
        |"phi", Normal -> Greek.Small.Normal.phi
        |"Phi", SansSerifBold  -> Greek.Capital.BoldSansSerif.phi
        |"Phi", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.phi
        |"phi", Bold  -> Greek.Small.Bold.phi
        |"phi", BoldItalic  -> Greek.Small.BoldItalic.phi
        |"phi", Italic  -> Greek.Small.Italic.phi
        |"phi", SansSerifBold  -> Greek.Small.BoldSansSerif.phi
        |"phi", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.phi
        |"phiSymbol", Normal -> Greek.Small.Normal.phiSymbol
        |"phiSymbol", Bold  -> Greek.Small.Bold.phiSymbol
        |"phiSymbol", BoldItalic  -> Greek.Small.BoldItalic.phiSymbol
        |"phiSymbol", Italic  -> Greek.Small.Italic.phiSymbol
        |"phiSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.phiSymbol
        |"phiSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.phiSymbol
        |"Pi", Bold  -> Greek.Capital.Bold.pi
        |"Pi", BoldItalic  -> Greek.Capital.BoldItalic.pi
        |"Pi", Italic  -> Greek.Capital.Italic.pi
        |"Pi", Normal  -> Greek.Capital.Normal.pi
        |"pi", Normal -> Greek.Small.Normal.pi
        |"Pi", SansSerifBold  -> Greek.Capital.BoldSansSerif.pi
        |"Pi", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.pi
        |"pi", Bold  -> Greek.Small.Bold.pi
        |"pi", BoldItalic  -> Greek.Small.BoldItalic.pi
        |"pi", Italic  -> Greek.Small.Italic.pi
        |"pi", SansSerifBold  -> Greek.Small.BoldSansSerif.pi
        |"pi", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.pi
        |"piSymbol", Normal -> Greek.Small.Normal.piSymbol
        |"piSymbol", Bold  -> Greek.Small.Bold.piSymbol
        |"piSymbol", BoldItalic  -> Greek.Small.BoldItalic.piSymbol
        |"piSymbol", Italic  -> Greek.Small.Italic.piSymbol
        |"piSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.piSymbol
        |"piSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.piSymbol
        |"Psi", Bold  -> Greek.Capital.Bold.psi
        |"Psi", BoldItalic  -> Greek.Capital.BoldItalic.psi
        |"Psi", Italic  -> Greek.Capital.Italic.psi
        |"Psi", Normal  -> Greek.Capital.Normal.psi
        |"psi", Normal -> Greek.Small.Normal.psi
        |"Psi", SansSerifBold  -> Greek.Capital.BoldSansSerif.psi
        |"Psi", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.psi
        |"psi", Bold  -> Greek.Small.Bold.psi
        |"psi", BoldItalic  -> Greek.Small.BoldItalic.psi
        |"psi", Italic  -> Greek.Small.Italic.psi
        |"psi", SansSerifBold  -> Greek.Small.BoldSansSerif.psi
        |"psi", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.psi
        |"q", Bold -> LatinSerif.Bold.q
        |"Q", Bold -> LatinSerif.Bold.Q
        |"q", BoldItalic -> LatinSerif.BoldItalic.q
        |"Q", BoldItalic -> LatinSerif.BoldItalic.Q
        |"q", DoubleStruck -> DoubleStruck.q
        |"Q", DoubleStruck -> DoubleStruck.Q
        |"q", Fraktur -> Fraktur.Normal.q
        |"Q", Fraktur -> Fraktur.Normal.Q
        |"q", FrakturBold -> Fraktur.Bold.q
        |"Q", FrakturBold -> Fraktur.Bold.Q
        |"q", Italic -> LatinSerif.Italic.q
        |"Q", Italic -> LatinSerif.Italic.Q
        |"q", LatinScript -> LatinScript.Normal.q
        |"Q", LatinScript -> LatinScript.Normal.Q
        |"q", LatinScriptBold -> LatinScript.Bold.q
        |"Q", LatinScriptBold -> LatinScript.Bold.Q
        |"q", MonoSpace -> MonoSpaced.q
        |"Q", MonoSpace -> MonoSpaced.Q
        |"q", Normal -> LatinSerif.Normal.q
        |"Q", Normal -> LatinSerif.Normal.Q
        |"q", SansSerif -> LatinSansSerif.Normal.q
        |"Q", SansSerif -> LatinSansSerif.Normal.Q
        |"q", SansSerifBold -> LatinSansSerif.Bold.q
        |"Q", SansSerifBold -> LatinSansSerif.Bold.Q
        |"q", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.q
        |"Q", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.Q
        |"q", SansSerifItalic -> LatinSansSerif.Italic.q
        |"Q", SansSerifItalic -> LatinSansSerif.Italic.Q
        |"r", Bold -> LatinSerif.Bold.r
        |"R", Bold -> LatinSerif.Bold.R
        |"r", BoldItalic -> LatinSerif.BoldItalic.r
        |"R", BoldItalic -> LatinSerif.BoldItalic.R
        |"r", DoubleStruck -> DoubleStruck.r
        |"R", DoubleStruck -> DoubleStruck.R
        |"r", Fraktur -> Fraktur.Normal.r
        |"R", Fraktur -> Fraktur.Normal.R
        |"r", FrakturBold -> Fraktur.Bold.r
        |"R", FrakturBold -> Fraktur.Bold.R
        |"r", Italic -> LatinSerif.Italic.r
        |"R", Italic -> LatinSerif.Italic.R
        |"r", LatinScript -> LatinScript.Normal.r
        |"R", LatinScript -> LatinScript.Normal.R
        |"r", LatinScriptBold -> LatinScript.Bold.r
        |"R", LatinScriptBold -> LatinScript.Bold.R
        |"r", MonoSpace -> MonoSpaced.r
        |"R", MonoSpace -> MonoSpaced.R
        |"r", Normal -> LatinSerif.Normal.r
        |"R", Normal -> LatinSerif.Normal.R
        |"r", SansSerif -> LatinSansSerif.Normal.r
        |"R", SansSerif -> LatinSansSerif.Normal.R
        |"r", SansSerifBold -> LatinSansSerif.Bold.r
        |"R", SansSerifBold -> LatinSansSerif.Bold.R
        |"r", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.r
        |"R", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.R
        |"r", SansSerifItalic -> LatinSansSerif.Italic.r
        |"R", SansSerifItalic -> LatinSansSerif.Italic.R
        |"Rho", Bold  -> Greek.Capital.Bold.rho
        |"Rho", BoldItalic  -> Greek.Capital.BoldItalic.rho
        |"Rho", Italic  -> Greek.Capital.Italic.rho
        |"Rho", Normal  -> Greek.Capital.Normal.rho
        |"rho", Normal -> Greek.Small.Normal.rho
        |"Rho", SansSerifBold  -> Greek.Capital.BoldSansSerif.rho
        |"Rho", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.rho
        |"rho", Bold  -> Greek.Small.Bold.rho
        |"rho", BoldItalic  -> Greek.Small.BoldItalic.rho
        |"rho", Italic  -> Greek.Small.Italic.rho
        |"rho", SansSerifBold  -> Greek.Small.BoldSansSerif.rho
        |"rho", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.rho
        |"rhoSymbol", Normal -> Greek.Small.Normal.RhoSymbol
        |"rhoSymbol", Bold  -> Greek.Small.Bold.RhoSymbol
        |"rhoSymbol", BoldItalic  -> Greek.Small.BoldItalic.RhoSymbol
        |"rhoSymbol", Italic  -> Greek.Small.Italic.RhoSymbol
        |"rhoSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.RhoSymbol
        |"rhoSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.RhoSymbol
        |"s", Bold -> LatinSerif.Bold.s
        |"S", Bold -> LatinSerif.Bold.S
        |"s", BoldItalic -> LatinSerif.BoldItalic.s
        |"S", BoldItalic -> LatinSerif.BoldItalic.S
        |"s", DoubleStruck -> DoubleStruck.s
        |"S", DoubleStruck -> DoubleStruck.S
        |"s", Fraktur -> Fraktur.Normal.s
        |"S", Fraktur -> Fraktur.Normal.S
        |"s", FrakturBold -> Fraktur.Bold.s
        |"S", FrakturBold -> Fraktur.Bold.S
        |"s", Italic -> LatinSerif.Italic.s
        |"S", Italic -> LatinSerif.Italic.S
        |"s", LatinScript -> LatinScript.Normal.s
        |"S", LatinScript -> LatinScript.Normal.S
        |"s", LatinScriptBold -> LatinScript.Bold.s
        |"S", LatinScriptBold -> LatinScript.Bold.S
        |"s", MonoSpace -> MonoSpaced.s
        |"S", MonoSpace -> MonoSpaced.S
        |"s", Normal -> LatinSerif.Normal.s
        |"S", Normal -> LatinSerif.Normal.S
        |"s", SansSerif -> LatinSansSerif.Normal.s
        |"S", SansSerif -> LatinSansSerif.Normal.S
        |"s", SansSerifBold -> LatinSansSerif.Bold.s
        |"S", SansSerifBold -> LatinSansSerif.Bold.S
        |"s", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.s
        |"S", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.S
        |"s", SansSerifItalic -> LatinSansSerif.Italic.s
        |"S", SansSerifItalic -> LatinSansSerif.Italic.S
        |"Sigma", Bold  -> Greek.Capital.Bold.sigma
        |"Sigma", BoldItalic  -> Greek.Capital.BoldItalic.sigma
        |"Sigma", Italic  -> Greek.Capital.Italic.sigma
        |"Sigma", Normal  -> Greek.Capital.Normal.sigma
        |"sigma", Normal -> Greek.Small.Normal.sigma
        |"Sigma", SansSerifBold  -> Greek.Capital.BoldSansSerif.sigma
        |"Sigma", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.sigma
        |"sigma", Bold  -> Greek.Small.Bold.sigma
        |"sigma", BoldItalic  -> Greek.Small.BoldItalic.sigma
        |"sigma", Italic  -> Greek.Small.Italic.sigma
        |"sigma", SansSerifBold  -> Greek.Small.BoldSansSerif.sigma
        |"sigma", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.sigma
        |"t", Bold -> LatinSerif.Bold.t
        |"T", Bold -> LatinSerif.Bold.T
        |"t", BoldItalic -> LatinSerif.BoldItalic.t
        |"T", BoldItalic -> LatinSerif.BoldItalic.T
        |"t", DoubleStruck -> DoubleStruck.t
        |"T", DoubleStruck -> DoubleStruck.T
        |"t", Fraktur -> Fraktur.Normal.t
        |"T", Fraktur -> Fraktur.Normal.T
        |"t", FrakturBold -> Fraktur.Bold.t
        |"T", FrakturBold -> Fraktur.Bold.T
        |"t", Italic -> LatinSerif.Italic.t
        |"T", Italic -> LatinSerif.Italic.T
        |"t", LatinScript -> LatinScript.Normal.t
        |"T", LatinScript -> LatinScript.Normal.T
        |"t", LatinScriptBold -> LatinScript.Bold.t
        |"T", LatinScriptBold -> LatinScript.Bold.T
        |"t", MonoSpace -> MonoSpaced.t
        |"T", MonoSpace -> MonoSpaced.T
        |"t", Normal -> LatinSerif.Normal.t
        |"T", Normal -> LatinSerif.Normal.T
        |"t", SansSerif -> LatinSansSerif.Normal.t
        |"T", SansSerif -> LatinSansSerif.Normal.T
        |"t", SansSerifBold -> LatinSansSerif.Bold.t
        |"T", SansSerifBold -> LatinSansSerif.Bold.T
        |"t", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.t
        |"T", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.T
        |"t", SansSerifItalic -> LatinSansSerif.Italic.t
        |"T", SansSerifItalic -> LatinSansSerif.Italic.T
        |"Tau", Bold  -> Greek.Capital.Bold.tau
        |"Tau", BoldItalic  -> Greek.Capital.BoldItalic.tau
        |"Tau", Italic  -> Greek.Capital.Italic.tau
        |"Tau", Normal  -> Greek.Capital.Normal.tau
        |"tau", Normal -> Greek.Small.Normal.tau
        |"Tau", SansSerifBold  -> Greek.Capital.BoldSansSerif.tau
        |"Tau", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.tau
        |"tau", Bold  -> Greek.Small.Bold.tau
        |"tau", BoldItalic  -> Greek.Small.BoldItalic.tau
        |"tau", Italic  -> Greek.Small.Italic.tau
        |"tau", SansSerifBold  -> Greek.Small.BoldSansSerif.tau
        |"tau", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.tau
        |"Theta", Bold  -> Greek.Capital.Bold.theta
        |"Theta", BoldItalic  -> Greek.Capital.BoldItalic.theta
        |"Theta", Italic  -> Greek.Capital.Italic.theta
        |"Theta", Normal  -> Greek.Capital.Normal.theta
        |"theta", Normal -> Greek.Small.Normal.theta
        |"Theta", SansSerifBold  -> Greek.Capital.BoldSansSerif.theta
        |"Theta", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.theta
        |"theta", Bold  -> Greek.Small.Bold.theta
        |"theta", BoldItalic  -> Greek.Small.BoldItalic.theta
        |"theta", Italic  -> Greek.Small.Italic.theta
        |"theta", SansSerifBold  -> Greek.Small.BoldSansSerif.theta
        |"theta", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.theta
        |"ThetaSymbol", Bold  -> Greek.Capital.Bold.thetaSymbol
        |"ThetaSymbol", BoldItalic  -> Greek.Capital.BoldItalic.thetaSymbol
        |"ThetaSymbol", Italic  -> Greek.Capital.Italic.thetaSymbol
        |"ThetaSymbol", Normal  -> Greek.Capital.Normal.thetaSymbol
        |"thetaSymbol", Normal -> Greek.Small.Normal.ThetaSymbol
        |"ThetaSymbol", SansSerifBold  -> Greek.Capital.BoldSansSerif.thetaSymbol
        |"ThetaSymbol", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.thetaSymbol
        |"thetaSymbol", Bold  -> Greek.Small.Bold.ThetaSymbol
        |"thetaSymbol", BoldItalic  -> Greek.Small.BoldItalic.ThetaSymbol
        |"thetaSymbol", Italic  -> Greek.Small.Italic.ThetaSymbol
        |"thetaSymbol", SansSerifBold  -> Greek.Small.BoldSansSerif.ThetaSymbol
        |"thetaSymbol", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.ThetaSymbol
        |"u", Bold -> LatinSerif.Bold.u
        |"U", Bold -> LatinSerif.Bold.U
        |"u", BoldItalic -> LatinSerif.BoldItalic.u
        |"U", BoldItalic -> LatinSerif.BoldItalic.U
        |"u", DoubleStruck -> DoubleStruck.u
        |"U", DoubleStruck -> DoubleStruck.U
        |"u", Fraktur -> Fraktur.Normal.u
        |"U", Fraktur -> Fraktur.Normal.U
        |"u", FrakturBold -> Fraktur.Bold.u
        |"U", FrakturBold -> Fraktur.Bold.U
        |"u", Italic -> LatinSerif.Italic.u
        |"U", Italic -> LatinSerif.Italic.U
        |"u", LatinScript -> LatinScript.Normal.u
        |"U", LatinScript -> LatinScript.Normal.U
        |"u", LatinScriptBold -> LatinScript.Bold.u
        |"U", LatinScriptBold -> LatinScript.Bold.U
        |"u", MonoSpace -> MonoSpaced.u
        |"U", MonoSpace -> MonoSpaced.U
        |"u", Normal -> LatinSerif.Normal.u
        |"U", Normal -> LatinSerif.Normal.U
        |"u", SansSerif -> LatinSansSerif.Normal.u
        |"U", SansSerif -> LatinSansSerif.Normal.U
        |"u", SansSerifBold -> LatinSansSerif.Bold.u
        |"U", SansSerifBold -> LatinSansSerif.Bold.U
        |"u", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.u
        |"U", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.U
        |"u", SansSerifItalic -> LatinSansSerif.Italic.u
        |"U", SansSerifItalic -> LatinSansSerif.Italic.U
        |"Upsilon", Bold  -> Greek.Capital.Bold.upsilon
        |"Upsilon", BoldItalic  -> Greek.Capital.BoldItalic.upsilon
        |"Upsilon", Italic  -> Greek.Capital.Italic.upsilon
        |"Upsilon", Normal  -> Greek.Capital.Normal.upsilon
        |"upsilon", Normal -> Greek.Small.Normal.upsilon
        |"Upsilon", SansSerifBold  -> Greek.Capital.BoldSansSerif.upsilon
        |"Upsilon", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.upsilon
        |"upsilon", Bold  -> Greek.Small.Bold.upsilon
        |"upsilon", BoldItalic  -> Greek.Small.BoldItalic.upsilon
        |"upsilon", Italic  -> Greek.Small.Italic.upsilon
        |"upsilon", SansSerifBold  -> Greek.Small.BoldSansSerif.upsilon
        |"upsilon", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.upsilon
        |"v", Bold -> LatinSerif.Bold.v
        |"V", Bold -> LatinSerif.Bold.V
        |"v", BoldItalic -> LatinSerif.BoldItalic.v
        |"V", BoldItalic -> LatinSerif.BoldItalic.V
        |"v", DoubleStruck -> DoubleStruck.v
        |"V", DoubleStruck -> DoubleStruck.V
        |"v", Fraktur -> Fraktur.Normal.v
        |"V", Fraktur -> Fraktur.Normal.V
        |"v", FrakturBold -> Fraktur.Bold.v
        |"V", FrakturBold -> Fraktur.Bold.V
        |"v", Italic -> LatinSerif.Italic.v
        |"V", Italic -> LatinSerif.Italic.V
        |"v", LatinScript -> LatinScript.Normal.v
        |"V", LatinScript -> LatinScript.Normal.V
        |"v", LatinScriptBold -> LatinScript.Bold.v
        |"V", LatinScriptBold -> LatinScript.Bold.V
        |"v", MonoSpace -> MonoSpaced.v
        |"V", MonoSpace -> MonoSpaced.V
        |"v", Normal -> LatinSerif.Normal.v
        |"V", Normal -> LatinSerif.Normal.V
        |"v", SansSerif -> LatinSansSerif.Normal.v
        |"V", SansSerif -> LatinSansSerif.Normal.V
        |"v", SansSerifBold -> LatinSansSerif.Bold.v
        |"V", SansSerifBold -> LatinSansSerif.Bold.V
        |"v", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.v
        |"V", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.V
        |"v", SansSerifItalic -> LatinSansSerif.Italic.v
        |"V", SansSerifItalic -> LatinSansSerif.Italic.V
        |"w", Bold -> LatinSerif.Bold.w
        |"W", Bold -> LatinSerif.Bold.W
        |"w", BoldItalic -> LatinSerif.BoldItalic.w
        |"W", BoldItalic -> LatinSerif.BoldItalic.W
        |"w", DoubleStruck -> DoubleStruck.w
        |"W", DoubleStruck -> DoubleStruck.W
        |"w", Fraktur -> Fraktur.Normal.w
        |"W", Fraktur -> Fraktur.Normal.W
        |"w", FrakturBold -> Fraktur.Bold.w
        |"W", FrakturBold -> Fraktur.Bold.W
        |"w", Italic -> LatinSerif.Italic.w
        |"W", Italic -> LatinSerif.Italic.W
        |"w", LatinScript -> LatinScript.Normal.w
        |"W", LatinScript -> LatinScript.Normal.W
        |"w", LatinScriptBold -> LatinScript.Bold.w
        |"W", LatinScriptBold -> LatinScript.Bold.W
        |"w", MonoSpace -> MonoSpaced.w
        |"W", MonoSpace -> MonoSpaced.W
        |"w", Normal -> LatinSerif.Normal.w
        |"W", Normal -> LatinSerif.Normal.W
        |"w", SansSerif -> LatinSansSerif.Normal.w
        |"W", SansSerif -> LatinSansSerif.Normal.W
        |"w", SansSerifBold -> LatinSansSerif.Bold.w
        |"W", SansSerifBold -> LatinSansSerif.Bold.W
        |"w", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.w
        |"W", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.W
        |"w", SansSerifItalic -> LatinSansSerif.Italic.w
        |"W", SansSerifItalic -> LatinSansSerif.Italic.W
        |"x", Bold -> LatinSerif.Bold.x
        |"X", Bold -> LatinSerif.Bold.X
        |"x", BoldItalic -> LatinSerif.BoldItalic.x
        |"X", BoldItalic -> LatinSerif.BoldItalic.X
        |"x", DoubleStruck -> DoubleStruck.x
        |"X", DoubleStruck -> DoubleStruck.X
        |"x", Fraktur -> Fraktur.Normal.x
        |"X", Fraktur -> Fraktur.Normal.X
        |"x", FrakturBold -> Fraktur.Bold.x
        |"X", FrakturBold -> Fraktur.Bold.X
        |"x", Italic -> LatinSerif.Italic.x
        |"X", Italic -> LatinSerif.Italic.X
        |"x", LatinScript -> LatinScript.Normal.x
        |"X", LatinScript -> LatinScript.Normal.X
        |"x", LatinScriptBold -> LatinScript.Bold.x
        |"X", LatinScriptBold -> LatinScript.Bold.X
        |"x", MonoSpace -> MonoSpaced.x
        |"X", MonoSpace -> MonoSpaced.X
        |"x", Normal -> LatinSerif.Normal.x
        |"X", Normal -> LatinSerif.Normal.X
        |"x", SansSerif -> LatinSansSerif.Normal.x
        |"X", SansSerif -> LatinSansSerif.Normal.X
        |"x", SansSerifBold -> LatinSansSerif.Bold.x
        |"X", SansSerifBold -> LatinSansSerif.Bold.X
        |"x", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.x
        |"X", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.X
        |"x", SansSerifItalic -> LatinSansSerif.Italic.x
        |"X", SansSerifItalic -> LatinSansSerif.Italic.X
        |"Xi", Bold  -> Greek.Capital.Bold.xi
        |"Xi", BoldItalic  -> Greek.Capital.BoldItalic.xi
        |"Xi", Italic  -> Greek.Capital.Italic.xi
        |"Xi", Normal  -> Greek.Capital.Normal.xi
        |"xi", Normal -> Greek.Small.Normal.xi
        |"Xi", SansSerifBold  -> Greek.Capital.BoldSansSerif.xi
        |"Xi", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.xi
        |"xi", Bold  -> Greek.Small.Bold.xi
        |"xi", BoldItalic  -> Greek.Small.BoldItalic.xi
        |"xi", Italic  -> Greek.Small.Italic.xi
        |"xi", SansSerifBold  -> Greek.Small.BoldSansSerif.xi
        |"xi", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.xi
        |"y", Bold -> LatinSerif.Bold.y
        |"Y", Bold -> LatinSerif.Bold.Y
        |"y", BoldItalic -> LatinSerif.BoldItalic.y
        |"Y", BoldItalic -> LatinSerif.BoldItalic.Y
        |"y", DoubleStruck -> DoubleStruck.y
        |"Y", DoubleStruck -> DoubleStruck.Y
        |"y", Fraktur -> Fraktur.Normal.y
        |"Y", Fraktur -> Fraktur.Normal.Y
        |"y", FrakturBold -> Fraktur.Bold.y
        |"Y", FrakturBold -> Fraktur.Bold.Y
        |"y", Italic -> LatinSerif.Italic.y
        |"Y", Italic -> LatinSerif.Italic.Y
        |"y", LatinScript -> LatinScript.Normal.y
        |"Y", LatinScript -> LatinScript.Normal.Y
        |"y", LatinScriptBold -> LatinScript.Bold.y
        |"Y", LatinScriptBold -> LatinScript.Bold.Y
        |"y", MonoSpace -> MonoSpaced.y
        |"Y", MonoSpace -> MonoSpaced.Y
        |"y", Normal -> LatinSerif.Normal.y
        |"Y", Normal -> LatinSerif.Normal.Y
        |"y", SansSerif -> LatinSansSerif.Normal.y
        |"Y", SansSerif -> LatinSansSerif.Normal.Y
        |"y", SansSerifBold -> LatinSansSerif.Bold.y
        |"Y", SansSerifBold -> LatinSansSerif.Bold.Y
        |"y", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.y
        |"Y", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.Y
        |"y", SansSerifItalic -> LatinSansSerif.Italic.y
        |"Y", SansSerifItalic -> LatinSansSerif.Italic.Y
        |"z", Bold -> LatinSerif.Bold.z
        |"Z", Bold -> LatinSerif.Bold.Z
        |"z", BoldItalic -> LatinSerif.BoldItalic.z
        |"Z", BoldItalic -> LatinSerif.BoldItalic.Z
        |"z", DoubleStruck -> DoubleStruck.z
        |"Z", DoubleStruck -> DoubleStruck.Z
        |"z", Fraktur -> Fraktur.Normal.z
        |"Z", Fraktur -> Fraktur.Normal.Z
        |"z", FrakturBold -> Fraktur.Bold.z
        |"Z", FrakturBold -> Fraktur.Bold.Z
        |"z", Italic -> LatinSerif.Italic.z
        |"Z", Italic -> LatinSerif.Italic.Z
        |"z", LatinScript -> LatinScript.Normal.z
        |"Z", LatinScript -> LatinScript.Normal.Z
        |"z", LatinScriptBold -> LatinScript.Bold.z
        |"Z", LatinScriptBold -> LatinScript.Bold.Z
        |"z", MonoSpace -> MonoSpaced.z
        |"Z", MonoSpace -> MonoSpaced.Z
        |"z", Normal -> LatinSerif.Normal.z
        |"Z", Normal -> LatinSerif.Normal.Z
        |"z", SansSerif -> LatinSansSerif.Normal.z
        |"Z", SansSerif -> LatinSansSerif.Normal.Z
        |"z", SansSerifBold -> LatinSansSerif.Bold.z
        |"Z", SansSerifBold -> LatinSansSerif.Bold.Z
        |"z", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.z
        |"Z", SansSerifBoldItalic -> LatinSansSerif.BoldItalic.Z
        |"z", SansSerifItalic -> LatinSansSerif.Italic.z
        |"Z", SansSerifItalic -> LatinSansSerif.Italic.Z
        |"Zeta", Bold  -> Greek.Capital.Bold.zeta
        |"Zeta", BoldItalic  -> Greek.Capital.BoldItalic.zeta
        |"Zeta", Italic  -> Greek.Capital.Italic.zeta
        |"Zeta", Normal  -> Greek.Capital.Normal.zeta
        |"zeta", Normal -> Greek.Small.Normal.zeta
        |"Zeta", SansSerifBold  -> Greek.Capital.BoldSansSerif.zeta
        |"Zeta", SansSerifBoldItalic  -> Greek.Capital.BoldItalicSansSerif.zeta
        |"zeta", Bold  -> Greek.Small.Bold.zeta
        |"zeta", BoldItalic  -> Greek.Small.BoldItalic.zeta
        |"zeta", Italic  -> Greek.Small.Italic.zeta
        |"zeta", SansSerifBold  -> Greek.Small.BoldSansSerif.zeta
        |"zeta", SansSerifBoldItalic  -> Greek.Small.BoldItalicSansSerif.zeta
        
        | _ -> string
