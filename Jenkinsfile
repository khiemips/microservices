pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh 'dotnet build KernelAPI'
      }
    }
    stage('Test') {
      steps {
        sh '''dotnet build KernelAPI.Tests
              dotnet test KernelAPI.Tests --logger "trx;logfilename=report.xml"'''
      }
    }
    stage('Package') {
      steps {
        sh '''docker-compose  \\
              -f "./docker-compose.yml" \\
              -f "./docker-compose.override.yml" \\
              -f "./docker-compose.vs.release.yml" \\
              --no-ansi \\
	            build \\
              --force-rm \\
	            --no-cache'''
        sh """
          docker login ${ACR_LOGINSERVER} -u ${ACR_USR} -p ${ACR_PSW}
          docker tag kernelapi ${ACR_IMAGE_URL}
          docker push ${ACR_IMAGE_URL}"""
      }
    }
    stage('Deploy') {
      steps {
        script{
          def fileName = "${env.WORKSPACE}/${APP_YAML}"
          def image = "${ACR_IMAGE_URL}"
          def yaml = readYaml file: fileName
          yaml.spec.template.spec.containers[0].image = image
          sh ('rm -f ' + fileName)
          writeYaml file: fileName, data: yaml
        }

        acsDeploy(azureCredentialsId: 'f97c9003-0d2e-4ae0-ba49-a928bdd1a6a0', 
                  resourceGroupName: 'ufab-microservices', 
                  containerService: 'ufabTestAKS | AKS', 
                  configFilePaths: "${APP_YAML}, ${APP_SVC_YAML}", 
                  sshCredentialsId: 'aks-ssh')
      }
    }
  }
  post {
    always {
      step([$class: 'MSTestPublisher', testResultsFile:"KernelAPI.Tests/TestResults/*.xml", failOnError: true, keepLongStdio: true])
    }
  }
  environment {
    ACR_ID = credentials('acr-credentials')
    ACR_USR = "${env.ACR_ID_USR}"
    ACR_PSW = "${env.ACR_ID_PSW}"
    APP_YAML = "kernelApi.yaml"
    APP_SVC_YAML = "kernelApiService.yaml"
    ACR_IMAGE_URL = "${ACR_LOGINSERVER}/kernelapi:${BUILD_NUMBER}"
  }
}