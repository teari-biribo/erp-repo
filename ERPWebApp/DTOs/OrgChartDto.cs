public class OrgChartDto
{
    public List<NodeDataDto>? NodeDataArray { get; set; }
}

public class NodeDataDto
{
    public int Key { get; set; } // The ID of the node (EmployeeId)
    public string Name { get; set; } // The display name of the employee
    public string Title { get; set; } // The employee's role title
    public int? Parent { get; set; } // The manager Id of the employee

}