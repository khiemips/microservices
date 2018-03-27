pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        build 'KernelApi/Build KernelApi'
      }
    }
    stage('Test') {
      steps {
        build 'KernelApi/Test KernelApi'
      }
    }
  }
}