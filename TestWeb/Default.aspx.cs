using System;

public partial class _Default : TestWeb.BasePage
{

    protected void btn_exception_Click(object sender, EventArgs e)
    {
        throw new Exception("This is an unhandled exception that goes to the error chain processor");
    }
}