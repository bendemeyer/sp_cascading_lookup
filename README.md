## About this project:

This is an adaptation of a solution from DataCogs that was built for SharePoint 2007. This solution installs two new field types, a parent drop down list, and a child drop down list. Both of these fieldtypes act as lookup columns, pulling their data from an existing SharePoint list in the current site collection.

Setting up a parent column involves specifying a subsite URL within the site collection to get the list, specifying the list to pull data from, and then picking which columns to use for the drop down list display and value.

Setting up the child column is the same as above, with one additional step. You will need to select a column in the child list to compare to the value of the parent column for the cascaded filtering to take place.

This solution should deploy quickly to any SharePoint farm, and should be usable as any other fieldtype within SharePoint. Check the Documentation.txt document for details on removing the solution or for adding the columns to a custom page or page layout for editing.

If you just want the WSP to deploy, it's /bin/Debug/BeenJammin.SharePoint.CascadingLookups.wsp
