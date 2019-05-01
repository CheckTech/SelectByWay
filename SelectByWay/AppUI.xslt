<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:frmwrk="Corel Framework Data">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <frmwrk:uiconfig>
    <frmwrk:applicationInfo userConfiguration="true" />
  </frmwrk:uiconfig>
  
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <!-- Each item is defined in the item section of the application uiConfig -->
  <xsl:template match="uiConfig/items">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>

      <!-- Define any new items here -->
      
      <!-- C# Test Line Tool -->
      <itemData guid="181dbca6-2c75-4b0b-8af2-dc3eb08a97d2"
                type="groupedRadioButton"
                currentActiveOfGroup="*Bind(DataSource=WAppDataSource;Path=ActiveTool;BindType=TwoWay)"
                enable="*Bind(DataSource=WAppDataSource;Path=ToolboxEnable)"
                identifier="181dbca6-2c75-4b0b-8af2-dc3eb08a97d2"/>

    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>