using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Data;
using BeenJammin.SharePoint.CascadingLookups;
using Microsoft.SharePoint;
using System.Web.UI;

namespace BeenJammin.SharePoint.WebControls
{
    public class ParentDropDownListFieldControl : BaseFieldControl
    {
        protected DropDownList ParentDropDownList;

        protected DropDownList ChildDropDownList;

        protected string parentSiteUrl;
        protected string parentListName;
        protected string parentListTextField;
        protected string parentListValueField;

      
        void ParentDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChildDropDownListFieldControl child = (ChildDropDownListFieldControl) FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent; 
            child.SetDataSource(ParentDropDownList.SelectedValue);
        }

        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;
            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        protected override string DefaultTemplateName
        {
            get
            {
                return "ParentDropDownListFieldControl";
            }
        }

        public override object Value
        {
            get
            {
                EnsureChildControls();
                return ParentDropDownList.SelectedValue;
            }

            set
            {
                EnsureChildControls();
                ParentDropDownList.SelectedValue = (string)this.ItemFieldValue;
                ChildDropDownListFieldControl child = (ChildDropDownListFieldControl)FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent;
                child.SetDataSource(ParentDropDownList.SelectedValue);
            }
        }

        public override void Focus()
        {
            EnsureChildControls();
            ParentDropDownList.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

            ParentDropDownList = (DropDownList)TemplateContainer.FindControl("ParentDropDownList");

            if (ParentDropDownList == null)
                throw new ArgumentException("ParentDropDownList is null. Problem with ParentDropDownListFieldControl.ascx file.");

            ParentDropDownList.TabIndex = TabIndex;
            ParentDropDownList.CssClass = CssClass;
            ParentDropDownList.ToolTip = Field.Title + " Parent";

            parentSiteUrl = Field.GetCustomProperty("ParentSiteUrl").ToString();
            parentListName = Field.GetCustomProperty("ParentListName").ToString();
            parentListTextField = Field.GetCustomProperty("ParentListTextField").ToString();
            parentListValueField = Field.GetCustomProperty("ParentListValueField").ToString();

            SPSite site = new SPSite(parentSiteUrl);
            SPList list = site.OpenWeb().Lists[parentListName];

            // populate it with the values from the central master page list.
            DataView dv = new DataView(list.Items.GetDataTable());
            if (!String.Equals(this.parentListTextField, this.parentListValueField))
            {
                String[] columnCollection = { this.parentListTextField, this.parentListValueField };
                this.ParentDropDownList.DataSource = dv.ToTable(true, columnCollection);
            }
            else
            {
                this.ParentDropDownList.DataSource = dv.ToTable(true, parentListTextField);
            }

            this.ParentDropDownList.DataTextField = parentListTextField;
            this.ParentDropDownList.DataValueField = parentListValueField;
            this.ParentDropDownList.DataBind();

            if (this.ListItem[Field.Id] != null)
            {
                ParentDropDownList.SelectedValue = this.ListItem[Field.Id].ToString();
            }

            this.ParentDropDownList.AutoPostBack = true;

            this.ParentDropDownList.SelectedIndexChanged += new EventHandler(ParentDropDownList_SelectedIndexChanged);

            ChildDropDownListFieldControl child = (ChildDropDownListFieldControl)FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent;
            child.SetDataSource(ParentDropDownList.SelectedValue);

            // populate it with the values from the central master page list.
            /*SPDataSource dataSource = new SPDataSource();

            dataSource.List = list;

            this.ParentDropDownList.DataSource = dataSource;
            this.ParentDropDownList.DataTextField = parentListTextField;
            this.ParentDropDownList.DataValueField = parentListValueField;
            this.ParentDropDownList.DataBind();

            this.ParentDropDownList.AutoPostBack = true;

            this.ParentDropDownList.SelectedIndexChanged += new EventHandler(ParentDropDownList_SelectedIndexChanged);

            ChildDropDownListFieldControl child = (ChildDropDownListFieldControl)FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent;
            child.SetDataSource(ParentDropDownList.SelectedValue);*/
        }


    }

}