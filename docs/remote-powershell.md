On the SharePoint server:
Enable-PSRemoting
Enable-WSManCredSSP ¨CRole Server
Set-Item WSMan:\localhost\Shell\MaxMemoryPerShellMB 1024

On Client:
Set-ExecutionPolicy RemoteSigned
Set-item wsman:localhost\client\trustedhosts -Value "MySharePointServer"
Enable-WSManCredSSP -Role client -DelegateComputer "MySharePointServer"

To validate remote powershell is working:
Enter-PSSession -ComputerName "MySharePointServer" -Credential domain\username