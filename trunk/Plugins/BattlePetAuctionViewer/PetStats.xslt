<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="//table[@class='raritytable']/tr"/>
        </xsl:copy>
    </xsl:template>
  <xsl:template match="tr">pets.Add("<xsl:value-of select="td[@class='name']/a"/>","<xsl:value-of select="td[@class='numpc']"/>");
  </xsl:template>
</xsl:stylesheet>
