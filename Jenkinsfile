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
    stage('Package') {
      steps {
        build 'Package KernelApi'
      }
    }
    stage('Deploy') {
      steps {
        build 'Deploy KernelApi'
      }
    }
  }
}