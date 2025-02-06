pipeline{
    agent any

    environment {
        S3_BUCKET = "awsbucketlogsjenkins1920"
        UPLOAD_FILE = ""
        AWS_REGION = "us-east-1"
        EMAIL_RECIPIENTS = ""
    }

    stages{
        stage("Checkout Code"){
            steps{
                script {
                    sh '''
                        echo "TEST"
                        if [ ! -d CheckoutDirectory ]; then
                            mkdir CheckoutDirectory
                        fi
                        cd CheckoutDirectory
                        if [ -d KubernetesTest ]; then
                           rm -rf KubernetesTest
                        fi
                        git clone https://github.com/hoatranh013/KubernetesTest.git
                        cd KubernetesTest
                        logContent=$(git log -p -n 1 --pretty=format:"%h - %an, %ar : %s")
                        cd ..
                        if [ ! -d LogRepository ]; then
                            mkdir LogRepository
                        fi
                        cd LogRepository
                        count=1
                        stop=0
                        echo "TEST"
                        while [ $stop -ne 1 ]; do
                            if [ ! -f "log_commit_${count}.txt" ]; then
                                echo "ABC"
                                touch "log_commit_${count}.txt"
                                echo "$logContent" >> "log_commit_${count}.txt"
                                echo "log_commit_${count}.txt" > upload_file.txt
                                echo "$UPLOAD_FILE"
                                stop=1
                            else
                                echo "XYZ"
                                count=$((count+1))
                            fi
                        done
                        echo "TEST"
                    '''
                }
            }
            post{
                success{
                    withAWS(credentials: 'id-upload-to-s3', region: 'us-east-1'){
                         sh '''
                           export AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID}
                           export AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY}
                          getUploadFile=$(cat CheckoutDirectory/LogRepository/upload_file.txt)
                           aws s3 cp CheckoutDirectory/LogRepository/${getUploadFile} s3://${S3_BUCKET}/CheckoutDirectory/LogRepository/${getUploadFile} --region ${AWS_REGION}
                        '''
                       echo 'Upload Logs Into S3 Successfully'
                    }
                }
            }
        }
        stage("Send Email Notification"){
            steps{
                sh '''
                    if [ ! -d "EmailNotificationFolder" ] then
                        mkdir "EmailNotificationFolder"
                    fi
                    cd EmailNotificationFolder
                    count=1
                    stop=0
                    while [ stop -ne 1 ]; do
                        if [ ! -f "email_notification_${count}.txt" ]; then
                            touch "email_notification_${count}.txt";
                            echo "chaunguyengiang2000@gmail.com" >> "email_notification_${count}.txt"
                            stop=1
                        else
                            count=$((count+1))
                        fi
                    done
                '''
            }
            post{
                always{
                    sh '''
                        cd EmailNotificationFolder
                        emailList=""
                        for item in $(ls)
                        do
                            emailList="${emailList}, $(cat ${item})"
                        done
                        echo "$emailList"
                    '''
                    emailext(
                        to: "${emailList}",
                        subject: "Build Success: ${env.JOB_NAME} - Build #${env.BUILD_NUMBER}",
                        body: """
                            Build Status: SUCCESS
                            Project: ${env.JOB_NAME}
                            Build URL: ${env.BUILD_URL}
                            Build Log: ${env.BUILD_URL}console
                        """
                    )
                }
            }
        }
    }
    post{
        always{
            echo "========always========"
        }
        success{
            echo "========pipeline executed successfully ========"
        }
        failure{
            echo "========pipeline execution failed========"
        }
    }
}