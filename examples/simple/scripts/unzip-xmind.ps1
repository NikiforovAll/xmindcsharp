Get-ChildItem ./xmind-test/tmp -Recurse | Remove-Item -Recurse
Copy-Item "xmind-test\\test.xmind" -Destination "xmind-test\\tmp\\test.zip"
Expand-Archive ./xmind-test/tmp/test.zip -DestinationPath ./xmind-test/tmp
