# .NET Database Migrator

Simple database migrator for .NET

## Overview

Some assumptions made going into the package:

* This is by no means the first migrator around, nor will it be the last. I wanted something fairly simple, straightforward, and more or less self-contained, that I had some control over, of the migrator itself, and of the migrations themselves.

* The facilitated SQL migrations are either valid or they are not. The underlying provider will tell us when it is not.

* Migrations are versioned depending on the .NET [System.Version](https://msdn.microsoft.com/en-us/library/system.version.aspx) class, as exposed via [VersionMigrationAttribute](http://github.com/mwpowellhtx/netdbmigrator/blob/master/src/Kingdom.Data.Migrator.Core/Attributes/VersionMigrationAttribute.cs).

* I've also included a [TimeStampMigrationAttribute](http://github.com/mwpowellhtx/netdbmigrator/blob/master/src/Kingdom.Data.Migrator.Core/Attributes/TimeStampMigrationAttribute.cs) which is based on [DateTime](http://msdn.microsoft.com/en-us/library/system.datetime.aspx), the though being that sometimes migration is easier by calendar schedules than actual version numbers. I have not tested this aspect yet as I tend to prefer versioning, but it's there if anyone would care to exercise it.

* As mentioned there are two migration strategies: Version and DateTime. I did not spend a lot of time handling silly error conditions. Generally, you would never mix your versioning strategies. Either run with Version or DateTime, but never both.

* I wanted a migrator that I could decide which revisions to downgrade or upgrade. This is done by specifying all (no parameter) or the boundary mark to which the migration occurs.

* The migrator does not dictate how to host the migrations. You can run them in a console app, during a unit test, or even during an MVC startup, as part of your main application or as a separate upgrade procedure. Your particular upgrade scenario is completely up to you.

* At present I am targeting .NET 4.5 because I've got it available and it was easy to get going. Please submit an issue and a pull request if you want additional targets.

* The SqlServer migrator generally runs against the [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.aspx) which as far as I know should support whatever version of SqlServer you are running. Please submit an issue and a pull request if you want additional providers, for instance MySQL, Postgres, etc.

* When running migrations, the migrator will ensure that the database exists prior to running the first migration. It will leave existing databases alone.

## Project Assumptions &amp; Goals

I should pause to consider a couple of project design goals, assumptions, etc.

* I set out provide a simple framework that facilitates well-controlled, versioned migrations. The framework does not protect you from mismanaging your migrations, versions, etc, but it does facilitate an orderly process for downgrading and upgrading your databases. However, overall, the framework is functioning as intended. I can identify migrations, they are reconciled with what has been applied, and can downgrade and upgrade database versions accordingly.

* Sql Server is the first provider I for which I included support. Eventually, I may extend that into common providers such as [MySQL](http://dev.mysql.com/). To my knowledge, this extensibility should work fairly seamlessly.

* I made a conscious decision not to support Fluent migrations. After performing a couple of migrations in which I had a lot of boilerplate, cookie-cutter code, I decided that a fluent migration sentence would make better sense. The past several commits at the time I am writing this reflect this ongoing effort.

* My Fluent goals are not to provide all or nothing feature completeness, much less feature parity. Rather, I will support incrementally folding in aspects, phrases, clauses, etc, even whole statements, as makes sense to do so. Contributions are definitely welcome. Along these lines, support for ``ALTER TABLE`` is provided as reflected in recent commits.

## Dependencies

* [EntityFramework](http://msdn.microsoft.com/en-us/data/ef.aspx)

## Examples

A typical production use case might be the following.

```C#
using (var runner = new SqlServerMigrationRunner<Version>(ConnectionString,
    typeof (SqlServerMigrationRunnerTests)))
{
    // Upgrades to the latest possible known version.
    runner.Up();
}
```

It is also possible to downgrade or upgrade to specified versions:

```C#
using (var runner = new SqlServerMigrationRunner<Version>(ConnectionString,
    typeof (SqlServerMigrationRunnerTests)))
{

#if DEBUG
    // Downgrades all versions to zero but only in debug mode.
    runner.Down();
#endif

    // Upgrades through version 10.0
    runner.Up(new Version(10, 0));
}
```

## Release Notes

<table>
  <tbody>
    <tr>
      <th>Version</th>
      <th>Description</th>
    </tr>
    <tr>
      <td valign=top>1.0.0</td>
      <td>Initial version.</td>
    </tr>
    <tr>
      <td valign=top>1.1.0</td>
      <td>
        <ul>
          <li>Migrators are now disposable. This is more useful since it affords a clear boundary when resources must be released, namely database contexts, connections, and so forth.</li>
          <li>Added unit tests.</li>
          <li>Fixed bug with Version encoding.</li>
          <li>Fixed up all bug determining previously applied migrations.</li>
          <li>Added ability to run more of an action on the migrator.</li>
        </ul>
      </td>
    </tr>
  </tbody>
</table>
