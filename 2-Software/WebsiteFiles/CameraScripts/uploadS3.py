from boto.s3.connection import S3Connection
 
AWS_KEY = 'AKIAJLPSHW7QWSCNY6MQ'
AWS_SECRET = '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'
aws_connection = S3Connection(AWS_KEY, AWS_SECRET)
bucket = aws_connection.get_bucket('detected-bikes')

from boto.s3.key import Key
k = Key(bucket)
k.key = 'lastbike.jpg'
k.set_contents_from_filename('/home/ubuntu/Desktop/Bike Detector 3.0/latest detection/latest_detection.png')
k.set_acl("public-read")
#for file_key in bucket.list():
#    print file_key.name
