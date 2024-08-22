using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGeneration.ChangeTracking.StateTest;

[TestClass]
public class CascadingListTrackingTest
{
    [TestMethod]
    public void Init()
    {
        var state = new State2();
        state.Groups = new ChangeTrackingList<TestGroup>
        {
            new TestGroup
            {
                Id = 0,
                Name = "0",
                Items = new ChangeTrackingList<TestItem>
                {
                    new TestItem {Id= 0, Text = "0"},
                    new TestItem {Id= 1, Text = "1"},
                }
            },
        };

        Assert.IsTrue(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);

        state.AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);


    }

    [TestMethod]
    public void SetItemProperty()
    {
        var state = new State2();
        state.Groups = new ChangeTrackingList<TestGroup>
        {
            new TestGroup
            {
                Id = 0,
                Name = "0",
                Items = new ChangeTrackingList<TestItem>
                {
                    new TestItem {Id= 0, Text = "0"},
                    new TestItem {Id= 1, Text = "1"},
                }
            },
        };
        state.AcceptChanges();
        state.Groups[0].Items[0].Text = "1";

        Assert.IsTrue(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);

        state.AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);

    }

    [TestMethod]
    public void AddItem()
    {
        var state = new State2();
        state.Groups = new ChangeTrackingList<TestGroup>
        {
            new TestGroup
            {
                Id = 0,
                Name = "0",
                Items = new ChangeTrackingList<TestItem>
                {
                    new TestItem {Id= 0, Text = "0"},
                    new TestItem {Id= 1, Text = "1"},
                }
            },
        };
        state.AcceptChanges();
        state.Groups[0].Items.Add(new TestItem { Text = "A", Id = 2, GroupId = 0 });

        Assert.IsTrue(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)state.Groups[0].Items[2]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[2]).IsCascadingChanged);

        state.AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)state).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)state.Groups[0].Items[1]).IsCascadingChanged);

    }

}

[ChangeTracking]
public partial class State2 : State<State2>
{
    public State2()
    {
        Groups = [];
    }

    public partial ChangeTrackingList<TestGroup> Groups { get; set; }
}

[ChangeTracking]
public partial class TestGroup
{
    public TestGroup()
    {
        Items = [];
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required partial string Name { get; set; }
    public partial ChangeTrackingList<TestItem> Items { get; set; }
}

[ChangeTracking]
public partial class TestItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public partial int GroupId { get; set; }
    public required partial string Text { get; set; }
}
