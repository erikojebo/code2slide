require 'rake'
require 'albacore'

class Project
  attr_accessor :name, :is_executable, :has_build_configuration_folders

  def bin_folder
    sub_directory 'bin'
  end

  def obj_folder
    sub_directory 'obj'
  end

  def release_folder
    sub_directory 'bin/Release'
  end

  def debug_folder
    sub_directory 'bin/Debug'
  end

  def assembly_name
    extension = is_executable ? 'exe' : 'dll'
    @name + '.' + extension
  end

  def release_assembly
    File.join(release_folder, assembly_name)
  end

  def debug_assembly
    File.join(debug_folder, assembly_name)
  end

  def bin_assembly
    File.join(bin_folder, assembly_name)
  end

  def initialize(name, solution_directory)
    @name = name
    @solution_directory = solution_directory
  end

  def add_final_slash(path)
    ends_with_slash = path =~ /\/$/
    ends_with_slash ? path : path + "/"
  end

  def sub_directory(relative_path)
    path = File.join(@solution_directory, name, relative_path)
    add_final_slash(path)
  end

  def copy_to_output_dirs(files)
    if :has_build_configuration_folders
      cp_r files, release_folder
      cp_r files, debug_folder
    else
      cp_r files, bin_folder
    end
  end

  def clean
    rm_r bin_folder if File.directory? bin_folder
    rm_r obj_folder if File.directory? obj_folder
  end
end

class Solution
  attr_accessor :name, :directory, :msbuild_path, :nunit_path, :lib_directory, :projects

  def add_project(name)
    @projects << Project.new(name, @directory)
  end

  def add_executable_project(name)
    project = Project.new(name, @directory)
    project.is_executable = true
    @projects << project
  end

  def clean
    @projects.each { |p| p.clean }
  end

  def copy_libs
    lib_files = File.join(@lib_directory, '.')

    @projects.each { |p| p.copy_to_output_dirs lib_files }
  end

  def solution_file
    File.join(@directory, @name + '.sln')
  end

  def initialize(name, directory)
    @name = name
    @directory = directory
    @projects = []
  end
end

module GosuAlbacore
  desc 'Remove build output from previous builds (bin and obj folders)'
  task :clean do
    puts 'cleaning solution'
    solution.clean
  end

  desc 'Build the solution with the release configuration'
  msbuild :build_release do |msb|
    puts 'building release'
    msb.path_to_command = solution.msbuild_path
    msb.properties :configuration => :Release, :architecture => :x86
    msb.targets :Build
    msb.solution = solution.solution_file
  end

  desc 'Build the solution with the debug configuration'
  msbuild :build_debug do |msb|
    puts 'building debug'
    msb.path_to_command = solution.msbuild_path
    msb.properties :configuration => :Debug, :architecture => :x86
    msb.targets :Build
    msb.solution = solution.solution_file
  end

  desc 'Copy lib files to output directories of the projects in the solution'
  task :copy_libs do
    puts 'copying libs'
    solution.copy_libs
  end

  desc 'Run NUnit tests for all projects in the solution'
  nunit :nunit do |nunit|
    puts 'running nunit tests'

    nunit.path_to_command = solution.nunit_path
    nunit.options '/framework:net-4.0'

    solution.projects.each do |p|
      nunit.assemblies << p.release_assembly
    end
  end
end
