﻿using System;

namespace OpenDentBusiness {
	///<summary>For HQ only. This table is not present in general release.  Groups employees according to the role so that we can show escalation lists.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true,IsMissingInGeneral=true)]
	//[CrudTable(IsMissingInGeneral=true)]
	public class PhoneEmpSubGroup:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long PhoneEmpSubGroupNum;
		///<summary>FK to PhoneEmpDefault.EmployeeNum.</summary>
		public long EmployeeNum;
		///<summary>Enum:PhoneEmpSubGroupType </summary>
		public PhoneEmpSubGroupType SubGroupType;
		///<summary>Order of escalation importantance. Employees are ranked 1-n in order of importance. 1 is most important, 'n' is least important.</summary>
		public int EscalationOrder;

		public PhoneEmpSubGroup() {
			
		}

		public PhoneEmpSubGroup(long employeeNum,PhoneEmpSubGroupType subGroupType,int escalationOrder) {
			this.EmployeeNum=employeeNum;
			this.SubGroupType=subGroupType;
			this.EscalationOrder=escalationOrder;
		}

		///<summary></summary>
		public PhoneEmpSubGroup Copy() {
			return (PhoneEmpSubGroup)this.MemberwiseClone();
		}
	}

	///<summary>Enum representing different escalation groups. Adding a new value will also create a new escalation tab in FormMap/FormMapHQ and FormPhoneEmpDefaultEscalationEdit.  But do NOT change the order or add a new value in the middle.</summary>
	public enum PhoneEmpSubGroupType {
		///<summary>Default view. Should always be first value if we ever need to change this enum.</summary>
		Escalation,
		///<summary>Eservices.</summary>
		Eservices,
		///<summary>Conversions.</summary>
		Conversions,
		///<summary>Management.</summary>
		Management,
		///<summary>Engineers.</summary>
		Engineers,
		///<summary>Clinical.</summary>
		Clinical,
		///<summary>Available. Shows the first tech who is available to take a phone call. Shows first in the list of tabs.</summary>
		Available,
		///<summary>Replication.</summary>
		Replication,
		///<summary>Database Engineers.</summary>
		DatabaseEngineers,
		///<summary>eRx.</summary>
		eRx,
	}

}

/* //Create Table Statment as if it were in the ConvertDbScript
	if(DataConnection.DBtype==DatabaseType.MySql) {
		command="DROP TABLE IF EXISTS phoneempsubgroup";
		Db.NonQ(command);
		command=@"CREATE TABLE phoneempsubgroup (
			PhoneEmpSubGroupNum bigint NOT NULL auto_increment PRIMARY KEY,
			EmployeeNum bigint NOT NULL,
			SubGroupType tinyint NOT NULL,
			EscalationOrder int NOT NULL,
			INDEX(EmployeeNum)
			) DEFAULT CHARSET=utf8";
		Db.NonQ(command);
	}
	else {//oracle
		command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE phoneempsubgroup'; EXCEPTION WHEN OTHERS THEN NULL; END;";
		Db.NonQ(command);
		command=@"CREATE TABLE phoneempsubgroup (
			PhoneEmpSubGroupNum number(20) NOT NULL,
			EmployeeNum number(20) NOT NULL,
			SubGroupType number(3) NOT NULL,
			EscalationOrder number(11) NOT NULL,
			CONSTRAINT phoneempsubgroup_PhoneEmpSubGr PRIMARY KEY (PhoneEmpSubGroupNum)
			)";
		Db.NonQ(command);
		command=@"CREATE INDEX phoneempsubgroup_EmployeeNum ON phoneempsubgroup (EmployeeNum)";
		Db.NonQ(command);
	}
*/
  
/*Copy original PhoneEmpSubGroup.EscalationOrder to new table.
INSERT INTO phoneempsubgroup (EmployeeNum, SubGroupType, EscalationOrder)
SELECT EmployeeNum, 0, EscalationOrder FROM phoneempdefault WHERE EscalationOrder>0;


Drop old column from table
//Do not run this query. instead the column is just deprecated. ALTER TABLE phoneempdefault DROP COLUMN EscalationOrder;*/