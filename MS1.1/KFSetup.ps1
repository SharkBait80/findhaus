$accountId = '<account-id>'
$roleName = '<iam-role-name>'
$s3BucketName = '<s3-bucket-name>'
$firehoseDeliveryStreamName = '<delivery-stream-name>'
$s3Bucket = Get-S3Bucket -BucketName $s3BucketName
if($s3Bucket -eq $null)
{
    New-S3Bucket -BucketName $s3BucketName
}
$role = (Get-IAMRoles | ? { $_.RoleName -eq $roleName })

if($role -eq $null)
{
    # Assume role policy allowing Firehose to assume a role
    $assumeRolePolicy = @"
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "",
      "Effect": "Allow",
      "Principal": {
        "Service": "firehose.amazonaws.com"
      },
      "Action": "sts:AssumeRole",
      "Condition": {
        "StringEquals": {
          "sts:ExternalId":"$accountId"
        }
      }
    }
  ]
}
"@

    $role = New-IAMRole -RoleName $roleName -AssumeRolePolicyDocument $assumeRolePolicy

    # Add managed policy AmazonKinesisFirehoseFullAccess to role
    Register-IAMRolePolicy -RoleName $roleName -PolicyArn 'arn:aws:iam::aws:policy/AmazonKinesisFirehoseFullAccess'

    # Add policy giving access to S3
    $s3AccessPolicy = @"
{
"Version": "2012-10-17",  
    "Statement":
    [    
        {      
            "Sid": "",      
            "Effect": "Allow",      
            "Action":
            [        
                "s3:AbortMultipartUpload",        
                "s3:GetBucketLocation",        
                "s3:GetObject",        
                "s3:ListBucket",        
                "s3:ListBucketMultipartUploads",        
                "s3:PutObject"
            ],      
            "Resource":
            [        
                "arn:aws:s3:::$s3BucketName",
                "arn:aws:s3:::$s3BucketName/*"		    
            ]    
        } 
    ]
}
"@

    Write-IAMRolePolicy -RoleName $roleName -PolicyName "S3Access" -PolicyDocument $s3AccessPolicy

    # Sleep to wait for the eventual consistency of the role creation
    Start-Sleep -Seconds 2
}

$s3Destination = New-Object Amazon.KinesisFirehose.Model.S3DestinationConfiguration
$s3Destination.BucketARN = "arn:aws:s3:::" + $s3Bucket.BucketName
$s3Destination.RoleARN = $role.Arn

New-KINFDeliveryStream -DeliveryStreamName $firehoseDeliveryStreamName -S3DestinationConfiguration $s3Destination 

Write-KINFRecord -DeliveryStreamName $firehoseDeliveryStreamName -Record_Text "test record"
