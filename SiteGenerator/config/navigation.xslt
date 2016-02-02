<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" />

    <xsl:param name="selectedArticleName">dtag</xsl:param>

  <xsl:template match="directory">
    <xsl:if test ="count(ancestor::node()) > 1">
    <p>
      <a href="../../pages/{page[1]/@name}/page_1.html">
        Directory  <xsl:value-of select="title"/> <xsl:value-of select="count(ancestor::node())" />
      </a>
    </p>
    </xsl:if>

    <xsl:if test=".//page[@name=$selectedArticleName]">
    
      <ul class="navigationdirectory">
        <xsl:apply-templates select="page"/>
        <xsl:apply-templates select="directory"/>
      </ul>

    </xsl:if>

  </xsl:template>

  <xsl:template match="page">
    <xsl:if test="@name = $selectedArticleName">
      <li class="articlelink selected">
        <a class="articlelink selected" href="../../pages/{@name}/page_1.html">
          Page <xsl:value-of select="@name"/> (selected)
        </a>
      </li>      
    </xsl:if>
    <xsl:if test="@name != $selectedArticleName">
      <li class="articlelink">
        <a class="articlelink" href="../../pages/{@name}/page_1.html">
          Page <xsl:value-of select="@name"/> 
        </a>
      </li>
    </xsl:if>
  </xsl:template>  
      
      
    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
  
</xsl:stylesheet>
