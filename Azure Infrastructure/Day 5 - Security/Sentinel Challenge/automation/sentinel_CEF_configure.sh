#!/bin/bash

# set -euo pipefail
IFS=$'\n\t'

declare -r USAGESTRING="Usage: sentinel_CEF_configure.sh -w <WORKSPACEID> -k <WORKSPACEKEY>"

# Verify the type of input and number of values
# Display an error message if the input is not correct
# Exit the shell script with a status of 1 using exit 1 command.
if [ $# -eq 0 ]; then
    echo $USAGESTRING 2>&1; exit 1; 
fi

# Initialize parameters specified from command line
while getopts ":w:k:" arg; do
    case "${arg}" in
        w) # Process -w (Workspace ID)
            WORKSPACEID=${OPTARG}
        ;;
        k) # Process -k (Workpsace Key)
            WORKSPACEKEY=${OPTARG}
        ;;
        \?)
            echo "Invalid options found: -$OPTARG."
            echo $USAGESTRING 2>&1; exit 1; 
        ;;
    esac
done
shift $((OPTIND-1))

declare -a siemEvents=( 
    "CEF:0|Microsoft|ATA|1.9.0.0|AbnormalSensitiveGroupMembershipChangeSuspiciousActivity|Abnormal modification of sensitive groups|5|start=DATETOKEN app=GroupMembershipChangeEvent suser=krbtgt msg=krbtgt has uncharacteristically modified sensitive group memberships. externalId=2024 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113d028ca1ec1250ca0491" 
    "CEF:0|Microsoft|ATA|1.9.0.0|LdapBruteForceSuspiciousActivity|Brute force attack using LDAP simple bind|5|start=DATETOKEN app=Ldap msg=10000 password guess attempts were made on 100 accounts from W2012R2-000000-Server. One account password was successfully guessed. externalId=2004 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114acb8ca1ec1250cacdcb" 
    "CEF:0|Microsoft|ATA|1.9.0.0|EncryptionDowngradeSuspiciousActivity|Encryption downgrade activity|5|start=DATETOKEN app=Kerberos msg=The encryption method of the TGT field of TGS_REQ message from W2012R2-000000-Server has been downgraded based on previously learned behavior. This may be a result of a Golden Ticket in-use on W2012R2-000000-Server. externalId=2009 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114f938ca1ec1250cafcfa" 
    "CEF:0|Microsoft|ATA|1.9.0.0|EncryptionDowngradeSuspiciousActivity|Encryption downgrade activity|5|start=DATETOKEN app=Kerberos msg=The encryption method of the Encrypted_Timestamp field of AS_REQ message from W2012R2-000000-Server has been downgraded based on previously learned behavior. This may be a result of a credential theft using Overpass-the-Hash from W2012R2-000000-Server. externalId=2010 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113eaf8ca1ec1250ca0883" 
    "CEF:0|Microsoft|ATA|1.9.0.0|EncryptionDowngradeSuspiciousActivity|Encryption downgrade activity|5|start=DATETOKEN app=Kerberos msg=The encryption method of the ETYPE_INFO2 field of KRB_ERR message from W2012R2-000000-Server has been downgraded based on previously learned behavior. This may be a result of a Skeleton Key on DC1. externalId=2011 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114e5c8ca1ec1250cafafe" 
    "CEF:0|Microsoft|ATA|1.9.0.0|HoneytokenActivitySuspiciousActivity|Honeytoken activity|5|start=DATETOKEN app=Kerberos suser=USR78982 msg=The following activities were performed by USR78982 LAST78982:\r\nAuthenticated from CLIENT1 using NTLM when accessing domain1.test.local\cifs on DC1. externalId=2014 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114ab88ca1ec1250ca7f76" 
    "CEF:0|Microsoft|ATA|1.9.0.0|PassTheHashSuspiciousActivity|Identity theft using Pass-the-Hash attack|10|start=DATETOKEN app=Ntlm suser=USR46829 LAST46829 msg=USR46829 LAST46829's hash was stolen from one of the computers previously logged into by USR46829 LAST46829 and used from W2012R2-000000-Server. externalId=2017 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114bb28ca1ec1250caf673" 
    "CEF:0|Microsoft|ATA|1.9.0.0|PassTheTicketSuspiciousActivity|Identity theft using Pass-the-Ticket attack|10|start=DATETOKEN app=Kerberos suser=Birdie Lamb msg=Birdie Lamb (Software Engineer)'s Kerberos tickets were stolen from W2012R2-000106-Server to W2012R2-000051-Server and used to access domain1.test.local\host. externalId=2018 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114b458ca1ec1250caf5b7" 
    "CEF:0|Microsoft|ATA|1.9.0.0|GoldenTicketSuspiciousActivity|Kerberos Golden Ticket activity|10|start=DATETOKEN app=Kerberos suser=Sonja Chadsey msg=Suspicious usage of Sonja Chadsey (Software Engineer)'s Kerberos ticket, indicating a potential Golden Ticket attack, was detected. externalId=2022 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114b168ca1ec1250caf556" 
    "CEF:0|Microsoft|ATA|1.9.0.0|RetrieveDataProtectionBackupKeySuspiciousActivity|Malicious data protection private information request|10|start=DATETOKEN app=LsaRpc shost=W2012R2-000000-Server msg=An unknown user performed 1 successful attempt from W2012R2-000000-Server to retrieve DPAPI domain backup key from DC1. externalId=2020 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114d858ca1ec1250caf983" 
    "CEF:0|Microsoft|ATA|1.9.0.0|DirectoryServicesReplicationSuspiciousActivity|Malicious replication of Directory Services|10|start=DATETOKEN app=Drsr shost=W2012R2-000000-Server msg=Malicious replication requests were successfully performed from W2012R2-000000-Server against DC1. outcome=Success externalId=2006 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114be18ca1ec1250caf6b8" 
    "CEF:0|Microsoft|ATA|1.9.0.0|ForgedPacSuspiciousActivity|Privilege escalation using forged authorization data|10|start=DATETOKEN app=Kerberos suser=triservice msg=triservice attempted to escalate privileges against DC1 from W2012R2-000000-Server by using forged authorization data. externalId=2013 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114a938ca1ec1250ca7f48" 
    "CEF:0|Microsoft|ATA|1.9.0.0|SamrReconnaissanceSuspiciousActivity|Reconnaissance using Directory Services queries|5|start=DATETOKEN app=Samr shost=W2012R2-000000-Server msg=The following directory services queries using SAMR protocol were attempted against DC1 from W2012R2-000000-Server:\r\nSuccessful query about Incoming Forest Trust Builders (Members of this group can create incoming, one-way trusts to this forest) in domain1.test.local externalId=2021 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114e758ca1ec1250cafb2e" 
    "CEF:0|Microsoft|ATA|1.9.0.0|AccountEnumerationSuspiciousActivity|Reconnaissance using account enumeration|5|start=DATETOKEN app=Kerberos shost=W2012R2-000000-Server msg=Suspicious account enumeration activity using Kerberos protocol, originating from W2012R2-000000-Server, was detected. The attacker performed a total of 100 guess attempts for account names, 1 guess attempt matched existing account names in Active Directory. externalId=2003 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113de58ca1ec1250ca06d8" 
    "CEF:0|Microsoft|ATA|1.9.0.0|DnsReconnaissanceSuspiciousActivity|Reconnaissance using DNS|5|start=DATETOKEN app=Dns shost=W2012R2-000000-Server msg=Suspicious DNS activity was observed, originating from W2012R2-000000-Server (which is not a DNS server) against DC1. externalId=2007 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113df08ca1ec1250ca074c" 
    "CEF:0|Microsoft|ATA|1.9.0.0|EnumerateSessionsSuspiciousActivity|Reconnaissance using SMB session enumeration|5|start=DATETOKEN app=SrvSvc shost=W2012R2-000000-Server msg=SMB session enumeration attempts failed from W2012R2-000000-Server against DC1. No accounts were exposed. externalId=2012 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114a788ca1ec1250ca7735" 
    "CEF:0|Microsoft|ATA|1.9.0.0|RemoteExecutionSuspiciousActivity|Remote execution attempt detected|5|start=DATETOKEN shost=W2012R2-000000-Server msg=The following remote execution attempts were performed on DC1 from W2012R2-000000-Server:\r\nFailed remote scheduling of one or more tasks. externalId=2019 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114c548ca1ec1250caf783" 
    "CEF:0|Microsoft|ATA|1.9.0.0|AbnormalProtocolSuspiciousActivity|Unusual protocol implementation|5|start=DATETOKEN app=Ntlm shost=W2012R2-000000-Server outcome=Success msg=triservice successfully authenticated from W2012R2-000000-Server against DC1 using an unusual protocol implementation. This may be a result of malicious tools used to execute attacks such as Pass-the-Hash and brute force. externalId=2002 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113c668ca1ec1250ca0397" 
    "CEF:0|Microsoft|ATA|1.9.0.0|AbnormalBehaviorSuspiciousActivity|Suspicion of identity theft based on abnormal behavior|5|start=DATETOKEN app=Kerberos suser=USR45964 msg=USR45964 LAST45964 exhibited abnormal behavior when performing activities that were not seen over the last month and are also not in accordance with the activities of other accounts in the organization. The abnormal behavior is based on the following activities:\r\nPerformed interactive login from 30 abnormal workstations.\r\nRequested access to 30 abnormal resources. externalId=2001 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113c5b8ca1ec1250ca0355" 
    "CEF:0|Microsoft|ATA|1.9.0.0|BruteForceSuspiciousActivity|Suspicious authentication failures|5|start=DATETOKEN app=Kerberos shost=W2012R2-000106-Server msg=Suspicious authentication failures indicating a potential brute-force attack were detected from W2012R2-000106-Server. externalId=2023 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c113f988ca1ec1250ca5810" 
    "CEF:0|Microsoft|ATA|1.9.0.0|MaliciousServiceCreationSuspiciousActivity|Suspicious service creation|5|start=DATETOKEN app=ServiceInstalledEvent shost=W2012R2-000000-Server msg=triservice created FakeService in order to execute potentially malicious commands on W2012R2-000000-Server. externalId=2026 cs1Label=url cs1=https://192.168.0.220/suspiciousActivity/5c114b2d8ca1ec1250caf577" 
)

EVENTCOUNT=${#siemEvents[@]}

dateNowRand() {
    echo `date -d "-$(( ( RANDOM % 3 )  + 1 )) days" +%FT%R.0000000Z`
}

getSiemEventString() {
    RANDEVENTPOSITION=$(( ( RANDOM % $EVENTCOUNT ) ))
    RANDEVENTSTRING="${siemEvents[RANDEVENTPOSITION]}"
    REPLACEMENTDATE="$(dateNowRand;)"
    echo "${RANDEVENTSTRING/DATETOKEN/$REPLACEMENTDATE}"
}

sudo wget https://raw.githubusercontent.com/Azure/Azure-Sentinel/master/DataConnectors/CEF/cef_installer.py&&sudo python cef_installer.py $WORKSPACEID $WORKSPACEKEY

while :
do
    logger -p local4.warn -t CEF "$(getSiemEventString;)"
    sleep 1s
done