pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        git(url: 'https://github.com/khiemips/microservices.git', branch: 'master')
        sh '''sudo dotnet restore KernelAPI
sudo dotnet build KernelAPI'''
      }
    }
    stage('Test') {
      steps {
        sh 'sudo dotnet test KernelAPI.Tests'
      }
    }
    stage('Package') {
      steps {
        sh 'docker-compose -f "./docker-compose.yml" -f "./docker-compose.override.yml" --no-ansi build --force-rm --no-cache'
      }
    }
    stage('Deploy') {
      steps {
        sh '''docker login parkenipsbuildhub.azurecr.io -u parkenipsbuildhub -p D2DIg5NBkL7sLaNzrptPjby8gAaT/BMh
docker tag kernelapi parkenipsbuildhub.azurecr.io/kernelapi
docker push parkenipsbuildhub.azurecr.io/kernelapi'''
        cleanWs(cleanWhenAborted: true, cleanWhenFailure: true, cleanWhenNotBuilt: true, cleanWhenSuccess: true, cleanWhenUnstable: true, cleanupMatrixParent: true)
      }
    }
  }
}