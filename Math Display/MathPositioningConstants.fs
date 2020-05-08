namespace Math.Presentation

module MathPositioningConstants =
    // ref. https://docs.microsoft.com/en-us/typography/opentype/spec/math
    // Suggested values are from the microsoft documentation, the used
    // values are from the STIX 2 Math font.

    (*
    The following naming convention are used for fields in the MathConstants table:
    
    Height — Specifies a distance from the main baseline.
    Kern — Represents a fixed amount of empty space to be introduced.
    Gap — Represents an amount of empty space that may need to be increased to meet certain criteria.
    Drop and Rise — Specifies the relationship between measurements of two elements to be positioned 
        relative to each other (but not necessarily in a stack-like manner) that must meet certain 
        criteria. For a Drop, one of the positioned elements has to be moved down to satisfy those 
        criteria; for a Rise, the movement is upwards.
    Shift — Defines a vertical shift applied to an element sitting on a baseline.
    Dist — Defines a distance between baselines of two elements.
    *)
// CONSTANTS    
    /// Percentage of scaling down for level 1 superscripts and subscripts. 
    /// Suggested value: 80%.
    let scriptPercentScaleDown = 70.
    
    /// Percentage of scaling down for level 2 (scriptScript) superscripts and 
    /// subscripts. Suggested value: 60%.
    let scriptScriptPercentScaleDown = 55.
    
    /// Minimum height required for a delimited expression (contained within 
    /// parentheses, etc.) to be treated as a sub-formula.      
    /// Suggested value: normal line height × 1.5.
    let delimitedSubFormulaMinHeight = 1325.
    
    /// Minimum height of n-ary operators (such as integral and summation) for 
    /// formulas in display mode (that is, appearing as standalone page elements, 
    /// not embedded inline within text).
    let displayOperatorMinHeight = 1800.
    
    /// White space to be left between math formulas to ensure proper line spacing. 
    /// For example, for applications that treat line gap as a part of line ascender, 
    /// formulas with ink going above (os2.sTypoAscender + os2.sTypoLineGap - MathLeading) 
    /// or with ink going below os2.sTypoDescender will result in increasing line height.
    let mathLeading = 150.
    
    /// Axis height of the font. In math typesetting, the term axis refers to a 
    /// horizontal reference line used for positioning elements in a formula. 
    /// The math axis is similar to but distinct from the baseline for regular  
    /// text layout.For example, in a simple equation, a minus symbol or fraction rule  
    /// would be on the axis,but a string for a variable name would be set on a baseline  
    /// that is offset from the axis. The axisHeight value determines the amount of that 
    /// offset.
    let axisHeight = 258.
    
    /// Maximum (ink) height of accent base that does not require raising the accents. 
    ///Suggested: x‑height of the font (os2.sxHeight) plus any possible overshots.
    let accentBaseHeight = 480.
    
    /// Maximum (ink) height of accent base that does not require flattening the accents. 
    /// Suggested: cap height of the font (os2.sCapHeight).
    let flattenedAccentBaseHeight = 656.
// SUB/SUPERSCRIPT   
    /// The standard shift down applied to subscript elements. Positive for moving in 
    /// the downward direction. Suggested: os2.ySubscriptYOffset.
    let subscriptShiftDown = 210.
    
    /// Maximum allowed height of the (ink) top of subscripts that does not require 
    /// moving subscripts further down. Suggested: 4/5 x- height.
    let subscriptTopMax = 368.
    
    /// Minimum allowed drop of the baseline of subscripts relative to the (ink) bottom 
    /// of the base. Checked for bases that are treated as a box or extended shape. 
    /// Positive for subscript baseline dropped below the base bottom.
    let subscriptBaselineDropMin = 160.
    
    /// Standard shift up applied to superscript elements. 
    /// Suggested: os2.ySuperscriptYOffset.
    let superscriptShiftUp = 360.
    
    /// Standard shift of superscripts relative to the base, in cramped style.
    let superscriptShiftUpCramped = 252.
    
    /// Minimum allowed height of the (ink) bottom of superscripts that does not require 
    /// moving subscripts further up. Suggested: ¼ x-height.
    let superscriptBottomMin = 120.
    
    /// Maximum allowed drop of the baseline of superscripts relative to the (ink) top 
    /// of the base. Checked for bases that are treated as a box or extended shape. 
    /// Positive for superscript baseline below the base top.
    let superscriptBaselineDropMax = 230.
    
    /// Minimum gap between the superscript and subscript ink. 
    /// Suggested: 4 × default rule thickness.
    let subSuperscriptGapMin = 150.
    
    /// The maximum level to which the (ink) bottom of superscript can be pushed to 
    /// increase the gap between superscript and subscript, before subscript starts 
    /// being moved down. Suggested: 4/5 x-height.
    let superscriptBottomMaxWithSubscript = 380.
    
    /// Extra white space to be added after each subscript and superscript. 
    /// Suggested: 0.5 pt for a 12 pt font.
    let spaceAfterScript = 40.
// LIMITS    
    /// Minimum gap between the (ink) bottom of the upper limit, and the (ink) 
    /// top of the base operator.
    let upperLimitGapMin = 135.
    
    /// Minimum distance between baseline of upper limit and 
    /// (ink) top of the base operator.
    let upperLimitBaselineRiseMin = 300.
    
    /// Minimum gap between (ink) top of the lower limit, and 
    /// (ink) bottom of the base operator.
    let lowerLimitGapMin = 135.
    
    /// Minimum distance between baseline of the lower limit and 
    /// (ink) bottom of the base operator.
    let lowerLimitBaselineDropMin = 670.
// STACKS
    /// Standard shift up applied to the top element of a stack.
    let stackTopShiftUp = 470.
    
    /// Standard shift up applied to the top element of a stack in display style.
    let stackTopDisplayStyleShiftUp = 780.
    
    /// Standard shift down applied to the bottom element of a stack. Positive for 
    /// moving in the downward direction.
    let stackBottomShiftDown = 385.
    
    /// Standard shift down applied to the bottom element of a stack in display style. 
    /// Positive for moving in the downward direction.
    let stackBottomDisplayStyleShiftDown = 690.
    
    /// Minimum gap between (ink) bottom of the top element of a stack, and 
    /// the (ink) top of the bottom element. Suggested: 3 × default rule thickness.
    let stackGapMin = 150.
    
    /// Minimum gap between (ink) bottom of the top element of a stack, and 
    /// the (ink) top of the bottom element in display style. 
    /// Suggested: 7 × default rule thickness.
    let stackDisplayStyleGapMin = 300.
    
    /// Standard shift up applied to the top element of the stretch stack.
    let stretchStackTopShiftUp = 800.
    
    /// Standard shift down applied to the bottom element of the stretch stack. 
    /// Positive for moving in the downward direction.
    let stretchStackBottomShiftDown = 590.
    
    /// Minimum gap between the ink of the stretched element, and 
    /// the (ink) bottom of the element above. Suggested: same value as upperLimitGapMin.
    let stretchStackGapAboveMin = 68.
    
    /// Minimum gap between the ink of the stretched element, and the (ink) top of 
    /// the element below. Suggested: same value as lowerLimitGapMin.
    let stretchStackGapBelowMin = 68.
// FRACTIONS    
    /// Standard shift up applied to the numerator.
    let fractionNumeratorShiftUp = 585.
    
    /// Standard shift up applied to the numerator in display style. 
    /// Suggested: same value as stackTopDisplayStyleShiftUp.
    let fractionNumeratorDisplayStyleShiftUp = 640.
    
    /// Standard shift down applied to the denominator. Positive for moving in 
    /// the downward direction.
    let fractionDenominatorShiftDown = 585.
    
    /// Standard shift down applied to the denominator in display style. Positive for 
    /// moving in the downward direction. 
    /// Suggested: same value as stackBottomDisplayStyleShiftDown.
    let fractionDenominatorDisplayStyleShiftDown = 640.
    
    /// Minimum tolerated gap between the (ink) bottom of 
    /// the numerator and the ink of the fraction bar. 
    /// Suggested: default rule thickness.
    let fractionNumeratorGapMin = 68.
    
    /// Minimum tolerated gap between the (ink) bottom of the numerator and the ink of 
    /// the fraction bar in display style. Suggested: 3 × default rule thickness.
    let fractionNumeratorDisplayStyleGapMin = 150.
    
    /// Thickness of the fraction bar. Suggested: default rule thickness.
    let fractionRuleThickness = 68.
    
    /// Minimum tolerated gap between the (ink) top of the denominator and the ink of 
    /// the fraction bar. Suggested: default rule thickness.
    let fractionDenominatorGapMin = 68.
    
    /// Minimum tolerated gap between the (ink) top of the denominator and the ink of 
    /// the fraction bar in display style. Suggested: 3 × default rule thickness.
    let fractionDenominatorDisplayStyleGapMin = 150.
    
    /// Horizontal distance between the top and bottom elements of a skewed fraction.
    let skewedFractionHorizontalGap = 350.
    
    /// Vertical distance between the ink of the top and bottom elements of 
    /// a skewed fraction.
    let skewedFractionVerticalGap = 68.
// OVER/UNDERBARS    
    /// Distance between the overbar and the (ink) top of he base. 
    /// Suggested: 3 × default rule thickness.
    let overbarVerticalGap = 175.
    
    /// Thickness of overbar. Suggested: default rule thickness.
    let overbarRuleThickness = 68.
    
    /// Extra white space reserved above the overbar. 
    /// Suggested: default rule thickness.
    let overbarExtraAscender = 68.
    
    /// Distance between underbar and (ink) bottom of the base. 
    /// Suggested: 3 × default rule thickness.
    let underbarVerticalGap = 175.
    
    /// Thickness of underbar. Suggested: default rule thickness.
    let underbarRuleThickness = 68.
    
    /// Extra white space reserved below the underbar. Always positive. 
    /// Suggested: default rule thickness.
    let underbarExtraDescender = 68.
// RADICALS   
    /// Space between the (ink) top of the expression and the bar over it. 
    /// Suggested: 1¼ default rule thickness.
    let radicalVerticalGap = 85.
    
    /// Space between the (ink) top of the expression and the bar over it. 
    /// Suggested: default rule thickness + ¼ x-height.
    let radicalDisplayStyleVerticalGap = 170.
    
    /// Thickness of the radical rule. This is the thickness of 
    /// the rule in designed or constructed radical signs. 
    /// Suggested: default rule thickness.
    let radicalRuleThickness = 68.
    
    /// Extra white space reserved above the radical. 
    /// Suggested: same value as radicalRuleThickness.
    let radicalExtraAscender = 78.
    
    /// Extra horizontal kern before the degree of a radical, if such is present.
    let radicalKernBeforeDegree = 65.
    
    /// Negative kern after the degree of a radical, if such is present. 
    /// Suggested: −10/18 of em.
    let radicalKernAfterDegree = -335.
    
    /// Height of the bottom of the radical degree, if such is present, 
    /// in proportion to the ascender of the radical sign. Suggested: 60%.
    let radicalDegreeBottomRaisePercent = 55.
// CONNECTORS    
    /// Minimum overlap of connecting glyphs during glyph construction, in design units.
    let minConnectorOverlap = 100.