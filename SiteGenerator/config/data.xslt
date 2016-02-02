<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE xsl:stylesheet [
    <!ENTITY nbsp "&#x00A0;">
    <!ENTITY hidden "data[@alias = 'umbracoNaviHide'] = 1">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" />

    <xsl:param name="selectedPageNo">1</xsl:param>
  
    <xsl:param name="navigationContent">
      <h3>Navigation</h3>
    </xsl:param>
  
  
    <xsl:template match="document">
      <html>
        <head>
          <meta http-equiv="X-UA-Compatible" content="IE=edge" />
          <title>
            <xsl:value-of select="title"/> Page <xsl:value-of select="$selectedPageNo" />
        </title>
          <link rel="stylesheet" type="text/css" href="../../css/style.css" media="all" />
        </head>
        <body>
          <div id="centralizer">
            
            <div id="head">

              <xsl:apply-templates select="gallery" />
              
            </div>

            <div id="content">

              <div id="navigation">
                <xsl:value-of select="$navigationContent"  disable-output-escaping="yes"  />
              </div>
              
              <div id="main">

                <xsl:apply-templates select="title" />

                <xsl:if test="$selectedPageNo=1">
                  <xsl:apply-templates select="teaser"/>
                </xsl:if>

                <xsl:apply-templates select="page[@pageno=$selectedPageNo]"/>

                <xsl:for-each select="page">
                  <a class="pagelink" href="page_{@pageno}.html">Link to page <xsl:value-of select="@pageno"/></a>&nbsp;
                </xsl:for-each>
                
              </div>

              <div id="attachments">
                <xsl:apply-templates select="attachments" />

              </div>
              
            </div>

            <div id="footer">
              Copyright © 2016
            </div>
            
          </div>
          
        </body>
        
      </html>

    </xsl:template>

  <xsl:template match="title">
    <h1 class="title">
      <xsl:apply-templates />
    </h1>
  </xsl:template>

  <xsl:template match="teaser">
    <p class="teaser">
      <xsl:apply-templates />
    </p>
  </xsl:template>
  
  <xsl:template match="page">
      <p class="copy">
        <xsl:apply-templates />
      </p>
  </xsl:template>

  <xsl:template match="text">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="inlinepagelink">
    <a class="inlinepagelink" href="../../pages/{@name}/page_1.html">
      <xsl:value-of select="." />
    </a>
  </xsl:template>

  <xsl:template match="inlineimage">
    <img class="inlineimage" src="img/{@src}" />
  </xsl:template>

  <xsl:template match="gallery">
    <ul class="imagelist">
      <xsl:apply-templates select="image"/>
    </ul>
  </xsl:template>

  <xsl:template match="image">
    <li>
      <img src="{@url}" alt="{@name}"/>
    </li>  
  </xsl:template>

  <xsl:template match="attachments">
    <h3>Attachments</h3>
    <table>
      <tr>
        <th>Name</th>
        <th>Type</th>
        <th>Size</th>
      </tr>
      <xsl:apply-templates select="attachment"/>
    </table>
  </xsl:template>

  <xsl:template match="attachment">
    <tr>
      <td>
        <xsl:value-of select="@url"/>
      </td>
      <td>
        <xsl:value-of select="@type"/>
      </td>
      <td>
        <xsl:value-of select="@size"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
