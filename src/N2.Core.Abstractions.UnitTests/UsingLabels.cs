using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class UsingLabels
{
    [TestMethod]
    public void EachPropertyReturnsItsOwnName()
    {
        Assert.AreEqual("ProcedureDefinitions", Labels.ProcedureDefinitions);
        Assert.AreEqual("OfficerDefinitions", Labels.OfficerDefinitions);
        Assert.AreEqual("DocumentDefinitions", Labels.DocumentDefinitions);
        Assert.AreEqual("ProcessDefinitions", Labels.ProcessDefinitions);
        Assert.AreEqual("Healthy", Labels.Healthy);
        Assert.AreEqual("Status", Labels.Status);
        Assert.AreEqual("Message", Labels.Message);
        Assert.AreEqual("Create", Labels.Create);
        Assert.AreEqual("Save", Labels.Save);
        Assert.AreEqual("Cancel", Labels.Cancel);
        Assert.AreEqual("Update", Labels.Update);
        Assert.AreEqual("Culture", Labels.Culture);
        Assert.AreEqual("DeleteConfirmation", Labels.DeleteConfirmation);
        Assert.AreEqual("Ok", Labels.Ok);
        Assert.AreEqual("Created", Labels.Created);
        Assert.AreEqual("Accepted", Labels.Accepted);
        Assert.AreEqual("Found", Labels.Found);
        Assert.AreEqual("NoContent", Labels.NoContent);
        Assert.AreEqual("Success", Labels.Success);
        Assert.AreEqual("NotModified", Labels.NotModified);
        Assert.AreEqual("Timeout", Labels.Timeout);
        Assert.AreEqual("NotFound", Labels.NotFound);
        Assert.AreEqual("NoAccess", Labels.NoAccess);
        Assert.AreEqual("NotAccepted", Labels.NotAccepted);
        Assert.AreEqual("InternalServerError", Labels.InternalServerError);
        Assert.AreEqual("NotImplemented", Labels.NotImplemented);
        Assert.AreEqual("Error", Labels.Error);
        Assert.AreEqual("Info", Labels.Info);
        Assert.AreEqual("Warning", Labels.Warning);
        Assert.AreEqual("SuccessfulRemoved", Labels.SuccessfulRemoved);
    }
}
