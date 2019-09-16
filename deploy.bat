sam package --profile default --template-file template.yaml --s3-bucket elgin-sg-builds --output-template-file packaged.yaml

aws cloudformation deploy --template-file packaged.yaml --stack-name FindHausService  --capabilities CAPABILITY_IAM --profile default --region ap-southeast-1